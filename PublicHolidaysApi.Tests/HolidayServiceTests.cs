using FluentAssertions;
using Moq;
using Moq.AutoMock;
using PublicHolidaysApi.Enums;
using PublicHolidaysApi.Helpers;
using PublicHolidaysApi.Models;
using PublicHolidaysApi.Models.Database;
using PublicHolidaysApi.Models.Enrico;
using PublicHolidaysApi.Services;
using PublicHolidaysApi.Services.Api;
using PublicHolidaysApi.Services.Database;
using PublicHolidaysApi.Tests.TestHelpers;

namespace PublicHolidaysApi.Tests;

public class HolidayServiceTests : TestsBase
{
    private readonly AutoMocker _mocker;
    private readonly HolidayService _service;
    private readonly Mock<IEnricoApiService> _mockEnricoApiService;
    private readonly Mock<IDatabaseService> _mockDatabaseService;

    public HolidayServiceTests()
    {
        _mocker = new AutoMocker();
        _mockEnricoApiService = _mocker.GetMock<IEnricoApiService>();
        _mockDatabaseService = _mocker.GetMock<IDatabaseService>();
        _service = _mocker.CreateInstance<HolidayService>();
    }

    [Fact]
    public async Task GetSupportedCountriesAsync_ShouldReturnCountriesFromDatabase_WhenAvailable()
    {
        var expectedCountries = new List<SupportedCountryEntity>
        {
            new() { CountryCode = TestData.LTU, CountryName = TestData.Lithuania},
            new() { CountryCode = TestData.CAN, CountryName = TestData.Canada }
        };

        _mockDatabaseService.Setup(ds => ds.GetSupportedCountriesAsync())
            .ReturnsAsync(expectedCountries);

        var result = await _service.GetSupportedCountriesAsync();

        result.SupportedCountries.Should().BeEquivalentTo(expectedCountries);
        _mockEnricoApiService.Verify(api => api.GetSupportedCountriesAsync(), Times.Never);
    }
    
    [Fact]
    public async Task GetSupportedCountriesAsync_ShouldFetchFromApi_WhenDatabaseIsEmpty()
    {
        var apiCountries = new List<EnricoSupportedCountriesDto>
        {
            CreateEnricoSupportedCountriesDto(TestData.Lithuania, TestData.LTU),
            CreateEnricoSupportedCountriesDto(TestData.Canada, TestData.CAN)
        };

        _mockDatabaseService.Setup(ds => ds.GetSupportedCountriesAsync())
            .ReturnsAsync(new List<SupportedCountryEntity>());
        _mockEnricoApiService.Setup(api => api.GetSupportedCountriesAsync())
            .ReturnsAsync(apiCountries);
        
        var expectedMappedDto = MappingHelper.ToSupportedCountriesDto(apiCountries);
        var result = await _service.GetSupportedCountriesAsync();

        result.Should().BeEquivalentTo(expectedMappedDto);
        _mockDatabaseService.Verify(ds => ds.AddSupportedCountriesAsync(It.IsAny<List<SupportedCountryEntity>>()), Times.Once);
    }
    
    [Fact]
    public async Task GetHolidaysAsync_ShouldReturnHolidaysFromDatabase_WhenAvailable()
    {
        var holidayDate = TestData.ChineseNewYearDate;
        var countryCode = TestData.CAN;
        
        List<HolidayEntity> expectedHolidays =
        [
            CreateHolidayEntity(holidayDate, TestData.ChineseNewYearNamesDict, countryCode)
        ];
        var expectedDto = MappingHelper.ToGroupedHolidaysDto(expectedHolidays);

        _mockDatabaseService.Setup(ds => ds.GetHolidaysAsync(CountryCode.From(countryCode), holidayDate.Year))
            .ReturnsAsync(expectedHolidays);

        var result = await _service.GetHolidaysAsync(CountryCode.From(countryCode), holidayDate.Year);

        result.Should().BeEquivalentTo(expectedDto);
        _mockEnricoApiService.Verify(api => api.GetHolidaysForYearAsync(CountryCode.From(countryCode), holidayDate.Year), Times.Never);
    }
    
    [Fact]
    public async Task GetHolidaysAsync_ShouldFetchFromApi_WhenDatabaseIsEmpty()
    {
        var holidayDate = new DateOnly(2024, 1, 1);
        var countryCode = CountryCode.From(TestData.LTU);

        List<EnricoHolidaysDto> apiHolidays =
        [
            CreateEnricoHolidaysDto(holidayDate, [TestData.NewYearLithuanian, TestData.NewYearEnglish], TestData.PublicHoliday)
        ]; 
        
        _mockDatabaseService.Setup(ds => ds.GetHolidaysAsync(countryCode, holidayDate.Year))
            .ReturnsAsync(new List<HolidayEntity>());
        _mockEnricoApiService.Setup(api => api.GetHolidaysForYearAsync(countryCode, holidayDate.Year))
            .ReturnsAsync(apiHolidays);

        var expectedMappedDto = MappingHelper.ToGroupedHolidaysDto(apiHolidays);

        var result = await _service.GetHolidaysAsync(countryCode, holidayDate.Year);

        result.Should().BeEquivalentTo(expectedMappedDto);
        _mockDatabaseService.Verify(ds => ds.AddHolidaysAsync(It.IsAny<List<HolidayEntity>>()), Times.Once);
    }
    
    [Fact]
    public async Task GetSpecificDayStatusAsync_ShouldReturnDayStatusFromDatabase_WhenAvailable()
    {
        var date = TestData.NewYearDate;
        var country = CountryCode.From(TestData.CAN);
        var expectedStatus = DayStatus.PublicHoliday;

        _mockDatabaseService.Setup(ds => ds.GetDayStatusAsync(country, date))
            .ReturnsAsync(new DayStatusEntity { CountryCode = country.Value, Date = date, Status = expectedStatus.ToString() });

        var result = await _service.GetSpecificDayStatusAsync(country, date);

        result.Should().Be(expectedStatus);
        _mockEnricoApiService.Verify(api => api.GetWorkDayStatusAsync(It.IsAny<CountryCode>(), It.IsAny<DateOnly>()), Times.Never);
    }
    
    [Fact]
    public async Task GetSpecificDayStatusAsync_ShouldFetchFromApi_WhenDatabaseIsEmpty()
    {
        var date = TestData.NewYearDate;
        var country = CountryCode.From(TestData.CAN);
        var apiStatus = DayStatus.FreeDay;

        _mockDatabaseService.Setup(ds => ds.GetDayStatusAsync(country, date))
            .ReturnsAsync(null as DayStatusEntity);
        _mockEnricoApiService.Setup(api => api.GetWorkDayStatusAsync(It.IsAny<CountryCode>(), It.IsAny<DateOnly>()))
            .ReturnsAsync(new EnricoWorkDayStatusDto { IsWorkDay = false });
        _mockEnricoApiService.Setup(api => api.GetPublicHolidayStatusAsync(It.IsAny<CountryCode>(), It.IsAny<DateOnly>()))
            .ReturnsAsync(new EnricoPublicHolidayStatusDto { IsPublicHoliday = false });

        var result = await _service.GetSpecificDayStatusAsync(country, date);

        result.Should().Be(apiStatus);
        _mockDatabaseService.Verify(ds => ds.AddDayStatusAsync(It.IsAny<DayStatusEntity>()), Times.Once);
    }
    
    [Fact]
    public async Task GetMaximumFreeDays_ShouldReturnMaxFreeDaysFromDatabase_WhenAvailable()
    {
        var year = TestData.NewYearDate.Year;
        var country = CountryCode.From(TestData.CAN);
        var expectedMaxFreeDays = 5;

        _mockDatabaseService.Setup(ds => ds.GetMaxConsecutiveFreeDaysAsync(country, year))
            .ReturnsAsync(new MaxConsecutiveFreeDaysEntity { CountryCode = country.Value, MaxConsecutiveDays = expectedMaxFreeDays, Year = year });

        var result = await _service.GetMaximumFreeDays(country, year);

        result.Should().Be(expectedMaxFreeDays);
        _mockEnricoApiService.Verify(api => api.GetHolidaysForYearAsync(It.IsAny<CountryCode>(), It.IsAny<int>()), Times.Never);
    }
    
    [Fact]
    public async Task GetMaximumFreeDays_ShouldFetchFromApi_WhenDatabaseIsEmpty()
    {
        var firstHoliday = TestData.ValentinesDayDate;
        var secondHoliday = TestData.NewYearDate;
        var country = CountryCode.From(TestData.LTU);
        List<EnricoHolidaysDto> apiHolidays =
        [
            CreateEnricoHolidaysDto(firstHoliday, [TestData.NewYearLithuanian, TestData.NewYearEnglish], TestData.PublicHoliday),
            CreateEnricoHolidaysDto(secondHoliday, [TestData.ValentinesDayLithuanian], TestData.PublicHoliday)
        ]; 
        
        _mockDatabaseService.Setup(ds => ds.GetMaxConsecutiveFreeDaysAsync(country, firstHoliday.Year))
            .ReturnsAsync(null as MaxConsecutiveFreeDaysEntity);
        _mockEnricoApiService.Setup(api => api.GetHolidaysForYearAsync(country, firstHoliday.Year))
            .ReturnsAsync(apiHolidays);

        (int firstPreviousWorkDay, int firstNextWorkDay, int secondPreviousWorkDay, int secondNextWorkDay) = (1, 4, 10, 15);
        
        _mockEnricoApiService.Setup(api => api.GetNextWorkDayAsync(country, firstHoliday, 1))
            .ReturnsAsync(new DateWithDayOfWeekDto{ Year = firstHoliday.Year, Month = firstHoliday.Month, Day =firstNextWorkDay , DayOfWeek = 0});
        
        _mockEnricoApiService.Setup(api => api.GetNextWorkDayAsync(country, firstHoliday, -1))
            .ReturnsAsync(new DateWithDayOfWeekDto{ Year = firstHoliday.Year, Month = firstHoliday.Month, Day = firstPreviousWorkDay, DayOfWeek = 0});
        
        _mockEnricoApiService.Setup(api => api.GetNextWorkDayAsync(country, secondHoliday, 1))
            .ReturnsAsync(new DateWithDayOfWeekDto{ Year = secondHoliday.Year, Month = secondHoliday.Month, Day = secondNextWorkDay, DayOfWeek = 0});
        
        _mockEnricoApiService.Setup(api => api.GetNextWorkDayAsync(country, secondHoliday, -1))
            .ReturnsAsync(new DateWithDayOfWeekDto{ Year = secondHoliday.Year, Month = secondHoliday.Month, Day = secondPreviousWorkDay, DayOfWeek = 0});

        var expectedMaxConsecutiveFreeDays = Math.Max(firstNextWorkDay - firstPreviousWorkDay,secondNextWorkDay - secondPreviousWorkDay) - 1; // Example value: between Dec 25 and Jan 1

        var result = await _service.GetMaximumFreeDays(country, firstHoliday.Year);

        result.Should().Be(expectedMaxConsecutiveFreeDays);
        _mockDatabaseService.Verify(ds => ds.AddMaxConsecutiveFreeDaysAsync(It.IsAny<MaxConsecutiveFreeDaysEntity>()), Times.Once);
    }
}
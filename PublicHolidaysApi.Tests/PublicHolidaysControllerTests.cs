using Moq;
using PublicHolidaysApi.Controllers;
using PublicHolidaysApi.Enums;
using PublicHolidaysApi.Models;
using PublicHolidaysApi.Models.Database;
using PublicHolidaysApi.Services;
using PublicHolidaysApi.Tests.TestHelpers;

namespace PublicHolidaysApi.Tests;

public class PublicHolidaysControllerTests
{
    private readonly PublicHolidaysController _controller;
    private readonly Mock<IHolidayService> _mockHolidayService;

    public PublicHolidaysControllerTests()
    {
        _mockHolidayService = new Mock<IHolidayService>();
        _controller = new PublicHolidaysController(_mockHolidayService.Object);
    }

    [Fact]
    public async Task GetSupportedCountries_ShouldSucceed_WhenServiceReturnsCountries()
    {
        var expectedCountries = new SupportedCountriesDto
        {
            SupportedCountries = [new SupportedCountryEntity { CountryCode = "US", CountryName = "United States" }]
        };

        _mockHolidayService.Setup(service => service.GetSupportedCountriesAsync()).ReturnsAsync(expectedCountries);

        var result = await _controller.GetSupportedCountries();

        result.ShouldBeOkObjectResultWithValue(expectedCountries);
    }

    [Fact]
    public async Task GetSupportedCountries_ShouldReturnStatusCode500_WhenExceptionIsThrown()
    {
        _mockHolidayService.Setup(service => service.GetSupportedCountriesAsync())
            .ThrowsAsync(new Exception("Service error"));

        var result = await _controller.GetSupportedCountries();

        result.ShouldReturnInternalServerError("Service error");
    }

    [Fact]
    public async Task GetDayStatus_ShouldSucceed_WhenServiceReturnsDayStatus()
    {
        var expectedDayStatus = DayStatus.PublicHoliday;
        _mockHolidayService.Setup(service => service.GetSpecificDayStatusAsync(It.IsAny<CountryCode>(), It.IsAny<DateOnly>()))
            .ReturnsAsync(expectedDayStatus);
        
        var result = await _controller.GetDayStatus(CountryCode.From(TestData.LTU), TestData.NewYearDate);

        result.ShouldBeOkObjectResultWithValue(expectedDayStatus);
    }
    
    [Fact]
    public async Task GetDayStatus_ShouldReturnStatusCode500_WhenExceptionIsThrown()
    {
        _mockHolidayService.Setup(service => service.GetSpecificDayStatusAsync(It.IsAny<CountryCode>(), It.IsAny<DateOnly>()))
            .ThrowsAsync(new Exception("Service error"));
    
        var result = await _controller.GetDayStatus(CountryCode.From(TestData.CAN), TestData.CarnivalDate);

        result.ShouldReturnInternalServerError("Service error");

    }
    
    [Fact]
    public async Task GetHolidaysAsync_ShouldSucceed_WhenServiceReturnsHolidays()
    {
        var newYearLocalizedNames = new List<LocalizedNamesDto>
        {
            new() { Lang = "en", Text = "New Year's Day" },
            new() { Lang = "es", Text = "Año Nuevo" }
        };
        var simplifiedHolidays = new HolidaysDto
        {
            Day = 1,
            LocalizedNames = newYearLocalizedNames
        };
        var holidaysByMonth = new Dictionary<string, List<HolidaysDto>>
        {
            { "January", new List<HolidaysDto> { simplifiedHolidays } }
        };
        
        var expectedHolidays = new GroupedHolidaysDto
        {
            HolidaysByMonth = holidaysByMonth
        };
    
        _mockHolidayService.Setup(service => service.GetHolidaysAsync(It.IsAny<CountryCode>(), It.IsAny<int>()))
            .ReturnsAsync(expectedHolidays);
    
        var result = await _controller.GetHolidaysAsync(CountryCode.From(TestData.LTU), 2024);

        result.ShouldBeOkObjectResultWithValue(expectedHolidays);
    }
    
    [Fact]
    public async Task GetHolidaysAsync_ShouldReturnStatusCode500_WhenExceptionIsThrown()
    {
        _mockHolidayService.Setup(service => service.GetHolidaysAsync(It.IsAny<CountryCode>(), It.IsAny<int>()))
            .ThrowsAsync(new Exception("Service error"));
    
        var result = await _controller.GetHolidaysAsync(CountryCode.From("USA"), 2024);

        result.ShouldReturnInternalServerError("Service error");
        
    }
    
    [Fact]
    public async Task GetMaximumFreeDays_ShouldSucceed_WhenServiceReturnsMaximumFreeDays()
    {
        var expectedMaxFreeDays = 6;
        _mockHolidayService.Setup(service => service.GetMaximumFreeDays(It.IsAny<CountryCode>(), It.IsAny<int>()))
            .ReturnsAsync(expectedMaxFreeDays);
    
        var result = await _controller.GetMaximumFreeDays(CountryCode.From("USA"), 2024);
    
        result.ShouldBeOkObjectResultWithValue(expectedMaxFreeDays);
    }
    
    [Fact]
    public async Task GetMaximumFreeDays_ShouldReturnStatusCode500_WhenExceptionIsThrown()
    {
        _mockHolidayService.Setup(service => service.GetMaximumFreeDays(It.IsAny<CountryCode>(), It.IsAny<int>()))
            .ThrowsAsync(new Exception("Service error"));
    
        var result = await _controller.GetMaximumFreeDays(CountryCode.From(TestData.LTU), 2024);
    
        result.ShouldReturnInternalServerError("Service error");
    }
}
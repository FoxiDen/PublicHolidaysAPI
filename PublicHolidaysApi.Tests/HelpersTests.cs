using FluentAssertions;
using FluentAssertions.Execution;
using PublicHolidaysApi.Helpers;
using PublicHolidaysApi.Models.Database;
using PublicHolidaysApi.Models.Enrico;
using PublicHolidaysApi.Tests.TestHelpers;

namespace PublicHolidaysApi.Tests;

public class HelpersTests : TestsBase
{
    [Fact]
    public void ToQueryString_Should_Return_Empty_When_Dictionary_Is_Empty()
    {
        var parameters = new Dictionary<string, string>();

        var result = parameters.ToQueryString();
        result.Should().BeEmpty();
    }

    [Fact]
    public void ToQueryString_Should_Return_Query_String_For_Populated_Dictionary()
    {
        var parameters = new Dictionary<string, string>
        {
            { "key1", "value1" },
            { "key2", "value2" }
        };

        var result = parameters.ToQueryString();
        result.Should().Be("?key1=value1&key2=value2");
    }

    [Fact]
    public void ToLocalizedNamesDtoList_Should_Convert_Dictionary_To_List_Of_LocalizedNamesDto()
    {
        var dictionary = new Dictionary<string, string>
        {
            { "en", "English" },
            { "es", "Spanish" }
        };

        var result = dictionary.ToLocalizedNamesDtoList();

        using (new AssertionScope())
        {
            result.Should().HaveCount(2);
            result[0].Lang.Should().Be("en");
            result[0].Text.Should().Be("English");
            result[1].Lang.Should().Be("es");
            result[1].Text.Should().Be("Spanish");
        }
    }
    
    [Fact]
    public void ToSupportedCountriesDto_Should_Map_List_Of_EnricoSupportedCountriesDto()
    {
        // Arrange
        List<EnricoSupportedCountriesDto> enricoSupportedCountries =
        [
            CreateEnricoSupportedCountriesDto(TestData.Lithuania, TestData.LTU),
            CreateEnricoSupportedCountriesDto(TestData.Canada, TestData.CAN)
        ];

        var result = MappingHelper.ToSupportedCountriesDto(enricoSupportedCountries);

        result.Should().NotBeNull();
        result.SupportedCountries.Should().HaveCount(2);
        result.SupportedCountries[0].CountryName.Should().Be(TestData.Lithuania);
        result.SupportedCountries[0].CountryCode.Should().Be(TestData.LTU);
        result.SupportedCountries[1].CountryName.Should().Be(TestData.Canada);
        result.SupportedCountries[1].CountryCode.Should().Be(TestData.CAN);
    }

    [Fact]
    public void ToGroupedHolidaysDto_Should_GroupHolidaysByMonth_ForEnricoHolidaysDto()
    {
        List<EnricoHolidaysDto> holidays = 
        [
            CreateEnricoHolidaysDto(TestData.ValentinesDayDate, [TestData.ValentinesDayLithuanian], TestData.PublicHoliday),
            CreateEnricoHolidaysDto(TestData.NewYearDate, [TestData.NewYearEnglish, TestData.NewYearLithuanian], TestData.Observance),
        ];
        
        var firstHolidayMonth = MappingHelper.GetMonthName(TestData.ValentinesDayDate.Month);
        var secondHolidayMonth = MappingHelper.GetMonthName(TestData.NewYearDate.Month);

        var result = MappingHelper.ToGroupedHolidaysDto(holidays);

        using (new AssertionScope())
        {
            result.HolidaysByMonth.Count.Should().Be(2);
            result.Should().NotBeNull();
            
            result.HolidaysByMonth.Should().ContainKey(firstHolidayMonth);
            result.HolidaysByMonth[firstHolidayMonth].Should().HaveCount(1);
            result.HolidaysByMonth[firstHolidayMonth].Single().Day.Should().Be(TestData.ValentinesDayDate.Day);
            result.HolidaysByMonth[firstHolidayMonth].Single().LocalizedNames.Should().HaveCount(1);
            result.HolidaysByMonth[firstHolidayMonth].Single().LocalizedNames[0].Should().Be(TestData.ValentinesDayLithuanian);

            result.HolidaysByMonth.Should().ContainKey(secondHolidayMonth);
            result.HolidaysByMonth[secondHolidayMonth].Should().HaveCount(1);
            result.HolidaysByMonth[secondHolidayMonth].Single().Day.Should().Be(TestData.NewYearDate.Day);
            result.HolidaysByMonth[secondHolidayMonth].Single().LocalizedNames.Should().HaveCount(2);
            result.HolidaysByMonth[secondHolidayMonth].Single().LocalizedNames.First().Should().Be(TestData.NewYearEnglish);
            result.HolidaysByMonth[secondHolidayMonth].Single().LocalizedNames.Last().Should().Be(TestData.NewYearLithuanian);
        }
    }

    [Fact]
    public void ToGroupedHolidaysDto_Should_Group_Holidays_By_Month_For_HolidayEntity()
    {
        List<HolidayEntity> holidays = 
        [
            CreateHolidayEntity(TestData.ChineseNewYearDate, TestData.ChineseNewYearNamesDict, TestData.LTU),
            CreateHolidayEntity(TestData.CarnivalDate, TestData.CarnivalNamesDict, TestData.CAN),
        ];
        var firstHolidayMonth = MappingHelper.GetMonthName(TestData.ChineseNewYearDate.Month);
        var secondHolidayMonth = MappingHelper.GetMonthName(TestData.CarnivalDate.Month);

        var result = MappingHelper.ToGroupedHolidaysDto(holidays);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.HolidaysByMonth.Count.Should().Be(2);
            
            result.HolidaysByMonth.Should().ContainKey(firstHolidayMonth);
            result.HolidaysByMonth[firstHolidayMonth].Should().HaveCount(1);
            result.HolidaysByMonth[firstHolidayMonth].Single().Day.Should().Be(TestData.ChineseNewYearDate.Day);
            result.HolidaysByMonth[firstHolidayMonth].Single().LocalizedNames.Count.Should().Be(TestData.ChineseNewYearNamesDict.Count);
            result.HolidaysByMonth[firstHolidayMonth].Single().LocalizedNames.Count.Should().Be(TestData.ChineseNewYearNamesDict.Count);
            
            result.HolidaysByMonth.Should().ContainKey(secondHolidayMonth);
            result.HolidaysByMonth[secondHolidayMonth].Should().HaveCount(1);
            result.HolidaysByMonth[secondHolidayMonth].Single().Day.Should().Be(TestData.CarnivalDate.Day);
            result.HolidaysByMonth[secondHolidayMonth].Single().LocalizedNames.Count.Should().Be(TestData.CarnivalNamesDict.Count);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public void GetMonthName_Should_Throw_ArgumentOutOfRangeException_For_Invalid_Month(int monthNumber)
    {
        Action act = () => MappingHelper.GetMonthName(monthNumber);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("Month number must be between 1 and 12. (Parameter 'monthNumber')");
    }
}
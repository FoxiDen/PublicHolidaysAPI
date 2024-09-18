using PublicHolidaysApi.Models;
using PublicHolidaysApi.Models.Database;
using PublicHolidaysApi.Models.Enrico;
using PublicHolidaysApi.Tests.TestHelpers;

namespace PublicHolidaysApi.Tests;

public class TestsBase
{
    protected EnricoSupportedCountriesDto CreateEnricoSupportedCountriesDto(string fullName, string countryCode)
    {
        return new EnricoSupportedCountriesDto
        {
            CountryCode = countryCode,
            Regions = [],
            HolidayTypes = [],
            FullName = fullName,
            FromDate = TestData.AnyDateBaseDto,
            ToDate = TestData.AnyDateBaseDto
        };
    }
    
    protected HolidayEntity CreateHolidayEntity(DateOnly date, Dictionary<string, string> localizedNames, string countryCode)
    {
        return new HolidayEntity
        {
            Date = date,
            LocalizedNames = localizedNames,
            CountryCode = countryCode
        };
    }
    
    protected EnricoHolidaysDto CreateEnricoHolidaysDto(DateOnly date, List<LocalizedNamesDto> names, string holidayType)
    {
        return new EnricoHolidaysDto
        {
            Date = new DateWithDayOfWeekDto
            {
                DayOfWeek = 0,
                Day = date.Day,
                Month = date.Month,
                Year = date.Year
            },
            Name = names,
            HolidayType = holidayType
        };
    }
}
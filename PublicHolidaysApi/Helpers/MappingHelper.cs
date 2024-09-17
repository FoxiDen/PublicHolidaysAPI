using System.Globalization;
using PublicHolidaysApi.Models;
using PublicHolidaysApi.Models.Database;
using PublicHolidaysApi.Models.Enrico;

namespace PublicHolidaysApi.Helpers
{
    public static class MappingHelper
    {
        public static SupportedCountriesDto ToSupportedCountriesDto(List<EnricoSupportedCountriesDto> supportedCountries)
        {
            var supportedCountriesList = supportedCountries.Select(x => new SupportedCountryEntity
            {
                CountryName = x.FullName,
                CountryCode= x.CountryCode
            }).ToList();

            return new SupportedCountriesDto { SupportedCountries = supportedCountriesList };
        }

        public static GroupedHolidaysDto ToGroupedHolidaysDto(List<EnricoHolidaysDto> holidays)
        {
            var groupedByMonth = holidays
                .GroupBy(holidaysDto => holidaysDto.Date.Month)
                .ToDictionary(
                    group => GetMonthName(group.Key),
                    group => group.Select(holiday => new SimplifiedHolidaysDto
                    {
                        Day = holiday.Date.Day,
                        LocalizedNames = holiday.Name
                    }).ToList()
                );

            return new GroupedHolidaysDto { HolidaysByMonth = groupedByMonth };
        }
        
        public static GroupedHolidaysDto ToGroupedHolidaysDto(List<HolidayEntity> holidays)
        {
            var groupedByMonth = holidays
                .GroupBy(holiday => holiday.Date.Month)
                .ToDictionary(
                    group => GetMonthName(group.Key),
                    group => group.Select(holiday => new SimplifiedHolidaysDto
                    {
                        Day = holiday.Date.Day,
                        LocalizedNames = holiday.LocalizedNames.ToLocalizedNamesDtoList()
                    }).ToList());

            return new GroupedHolidaysDto { HolidaysByMonth = groupedByMonth };
        }

        public static List<HolidayEntity> ToHolidayEntityList(List<EnricoHolidaysDto> enricoHolidays, string countryCode)
        {
            return enricoHolidays
                .Select(holidayDto => new HolidayEntity
                {
                    CountryCode = countryCode,
                    Date = new DateOnly(holidayDto.Date.Year, holidayDto.Date.Month, holidayDto.Date.Day),
                    LocalizedNames = holidayDto.Name.ToDictionary(x=>x.Lang, x=>x.Text)
                })
                .ToList();
        }
        
        private static string GetMonthName(int monthNumber)
        {
            if (monthNumber < 1 || monthNumber > 12)
                throw new ArgumentOutOfRangeException(nameof(monthNumber), "Month number must be between 1 and 12.");

            // Using CultureInfo to get month names
            var culture = CultureInfo.InvariantCulture;
            return culture.DateTimeFormat.GetMonthName(monthNumber);
        }
    }
}
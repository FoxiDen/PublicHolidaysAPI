using System.Globalization;
using PublicHolidaysApi.Models;

namespace PublicHolidaysApi.Helpers
{
    public static class MappingHelper
    {
        public static SupportedCountriesCodesDto MapToSupportedCountryCodeList(List<EnricoSupportedCountriesDto> supportedCountries)
        {
            var supportedCountriesList = supportedCountries.Select(x => new SupportedCountryCode
            {
                Name = x.FullName,
                Code = CountryCode.From(x.CountryCode)
            }).ToList();

            return new SupportedCountriesCodesDto { SupportedCountries = supportedCountriesList };
        }

        public static GroupedHolidaysDto MapToGroupedHolidaysByMonth(List<EnricoHolidaysDto> holidays)
        {
            var groupedByMonth = holidays
                .GroupBy(holidaysDto => holidaysDto.Date.Month)
                .ToDictionary(
                    group => GetMonthName(group.Key),
                    group => group.Select(holiday => new SimplifiedHolidaysDto
                    {
                        Day = holiday.Date.Day,
                        Name = holiday.Name
                    }).ToList()
                );

            return new GroupedHolidaysDto { HolidaysByMonth = groupedByMonth };
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
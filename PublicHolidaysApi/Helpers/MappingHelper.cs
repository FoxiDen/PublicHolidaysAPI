using System.Globalization;
using PublicHolidaysApi.Models;
using PublicHolidaysApi.Models.Database;
using PublicHolidaysApi.Models.Enrico;

namespace PublicHolidaysApi.Helpers
{
    /// <summary>
    /// Provides helper methods for mapping between different data models.
    /// </summary>
    public static class MappingHelper
    {
        /// <summary>
        /// Converts a list of <see cref="EnricoSupportedCountriesDto"/> to a <see cref="SupportedCountriesDto"/>.
        /// </summary>
        public static SupportedCountriesDto ToSupportedCountriesDto(List<EnricoSupportedCountriesDto> supportedCountries)
        {
            var supportedCountriesList = supportedCountries.Select(x => new SupportedCountryEntity
            {
                CountryName = x.FullName,
                CountryCode= x.CountryCode
            }).ToList();

            return new SupportedCountriesDto { SupportedCountries = supportedCountriesList };
        }

        /// <summary>
        /// Converts a list of <see cref="EnricoHolidaysDto"/> to a <see cref="GroupedHolidaysDto"/>.
        /// </summary>
        public static GroupedHolidaysDto ToGroupedHolidaysDto(List<EnricoHolidaysDto> holidays)
        {
            var groupedByMonth = holidays
                .GroupBy(holidaysDto => holidaysDto.Date.Month)
                .ToDictionary(
                    group => GetMonthName(group.Key),
                    group => group.Select(holiday => new HolidaysDto
                    {
                        Day = holiday.Date.Day,
                        LocalizedNames = holiday.Name
                    }).ToList()
                );

            return new GroupedHolidaysDto { HolidaysByMonth = groupedByMonth };
        }
        
        /// <summary>
        /// Converts a list of <see cref="HolidayEntity"/> to a <see cref="GroupedHolidaysDto"/>.
        /// </summary>
        public static GroupedHolidaysDto ToGroupedHolidaysDto(List<HolidayEntity> holidays)
        {
            var groupedByMonth = holidays
                .GroupBy(holiday => holiday.Date.Month)
                .ToDictionary(
                    group => GetMonthName(group.Key),
                    group => group.Select(holiday => new HolidaysDto
                    {
                        Day = holiday.Date.Day,
                        LocalizedNames = holiday.LocalizedNames.ToLocalizedNamesDtoList()
                    }).ToList());

            return new GroupedHolidaysDto { HolidaysByMonth = groupedByMonth };
        }

        /// <summary>
        /// Converts a list of <see cref="EnricoHolidaysDto"/> to a list of <see cref="HolidayEntity"/>.
        /// </summary>
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
        
        internal static string GetMonthName(int monthNumber)
        {
            if (monthNumber < 1 || monthNumber > 12)
                throw new ArgumentOutOfRangeException(nameof(monthNumber), "Month number must be between 1 and 12.");

            var culture = CultureInfo.InvariantCulture;
            return culture.DateTimeFormat.GetMonthName(monthNumber);
        }
    }
}
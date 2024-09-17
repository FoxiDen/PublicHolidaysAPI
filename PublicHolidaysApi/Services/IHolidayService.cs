using PublicHolidaysApi.Enums;
using PublicHolidaysApi.Models;

namespace PublicHolidaysApi.Services;

public interface IHolidayService
{
    Task<SupportedCountriesDto> GetSupportedCountriesAsync();
    Task<DayStatus> GetSpecificDayStatusAsync(CountryCode country, DateOnly date);
    Task<GroupedHolidaysDto> GetHolidaysAsync(CountryCode countryCode, int year);
    Task<int> GetMaximumFreeDays(CountryCode country, int year);
}
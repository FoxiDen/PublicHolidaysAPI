using PublicHolidaysApi.Enums;
using PublicHolidaysApi.Models;

namespace PublicHolidaysApi.Services;

public interface IHolidayService
{
    Task<SupportedCountriesCodesDto> GetSupportedCountriesAsync();
    Task<DayStatus> GetSpecificDayStatusAsync(CountryCode country, DateTime date);
    Task<GroupedHolidaysDto> GetHolidaysAsync(CountryCode country, int year);
    Task<int> GetMaximumFreeDays(CountryCode country, int year);
}
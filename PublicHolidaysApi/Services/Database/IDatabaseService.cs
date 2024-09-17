using PublicHolidaysApi.Models;
using PublicHolidaysApi.Models.Database;

namespace PublicHolidaysApi.Services.Database;

public interface IDatabaseService
{
    Task<List<SupportedCountryEntity>> GetSupportedCountriesAsync();
    Task AddSupportedCountriesAsync(IEnumerable<SupportedCountryEntity> countries);

    Task<List<HolidayEntity>> GetHolidaysAsync(CountryCode countryCode, int year);
    Task AddHolidaysAsync(IEnumerable<HolidayEntity> holidays);

    Task<DayStatusEntity?> GetDayStatusAsync(CountryCode country, DateOnly date);
    Task AddDayStatusAsync(DayStatusEntity dayStatus);

    Task<MaxConsecutiveFreeDaysEntity?> GetMaxConsecutiveFreeDaysAsync(CountryCode country, int year);
    Task AddMaxConsecutiveFreeDaysAsync(MaxConsecutiveFreeDaysEntity maxFreeDays);
}
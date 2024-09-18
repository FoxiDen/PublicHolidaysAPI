using PublicHolidaysApi.Models;
using PublicHolidaysApi.Models.Database;

namespace PublicHolidaysApi.Services.Database;

/// <summary>
/// Service for interacting with the database.
/// </summary>
public interface IDatabaseService
{
    /// <summary>
    /// Retrieves the list of supported countries.
    /// </summary>
    Task<List<SupportedCountryEntity>> GetSupportedCountriesAsync();
    
    /// <summary>
    /// Adds new supported countries to the database.
    /// </summary>
    /// <param name="countries">The countries to add.</param>
    Task AddSupportedCountriesAsync(IEnumerable<SupportedCountryEntity> countries);

    /// <summary>
    /// Retrieves holidays for a specific country and year.
    /// </summary>
    /// <param name="countryCode">The country code.</param>
    /// <param name="year">The year to query.</param>
    Task<List<HolidayEntity>> GetHolidaysAsync(CountryCode countryCode, int year);
    
    /// <summary>
    /// Adds new holidays to the database.
    /// </summary>
    /// <param name="holidays">The holidays to add.</param>
    Task AddHolidaysAsync(IEnumerable<HolidayEntity> holidays);

    /// <summary>
    /// Retrieves the status of a specific day for a country.
    /// </summary>
    /// <param name="country">The country code.</param>
    /// <param name="date">The date to check.</param>
    Task<DayStatusEntity?> GetDayStatusAsync(CountryCode country, DateOnly date);
    
    /// <summary>
    /// Adds a day status to the database.
    /// </summary>
    /// <param name="dayStatus">The day status to add.</param>
    Task AddDayStatusAsync(DayStatusEntity dayStatus);

    /// <summary>
    /// Retrieves the maximum consecutive free days for a country and year.
    /// </summary>
    /// <param name="country">The country code.</param>
    /// <param name="year">The year to query.</param>
    Task<MaxConsecutiveFreeDaysEntity?> GetMaxConsecutiveFreeDaysAsync(CountryCode country, int year);
    
    /// <summary>
    /// Adds maximum consecutive free days data to the database.
    /// </summary>
    /// <param name="maxFreeDays">The max consecutive free days data to add.</param>
    Task AddMaxConsecutiveFreeDaysAsync(MaxConsecutiveFreeDaysEntity maxFreeDays);
}
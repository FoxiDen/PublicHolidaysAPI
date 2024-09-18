using PublicHolidaysApi.Enums;
using PublicHolidaysApi.Models;

namespace PublicHolidaysApi.Services;

/// <summary>
/// Service for retrieving holiday-related data.
/// </summary>
public interface IHolidayService
{
    /// <summary>
    /// Gets the supported countries.
    /// </summary>
    /// <returns>A task that returns the supported countries data.</returns>
    Task<SupportedCountriesDto> GetSupportedCountriesAsync();
    
    /// <summary>
    /// Gets the status of a specific day for a country.
    /// </summary>
    /// <param name="country">The country code.</param>
    /// <param name="date">The date to check.</param>
    /// <returns>A task that returns the day status.</returns>
    Task<DayStatus> GetSpecificDayStatusAsync(CountryCode country, DateOnly date);
    
    /// <summary>
    /// Gets holidays for a country and year.
    /// </summary>
    /// <param name="countryCode">The country code.</param>
    /// <param name="year">The year to check.</param>
    /// <returns>A task that returns holidays grouped by month.</returns>
    Task<GroupedHolidaysDto> GetHolidaysAsync(CountryCode countryCode, int year);
    
    /// <summary>
    /// Gets the maximum consecutive free days for a country and year.
    /// </summary>
    /// <param name="country">The country code.</param>
    /// <param name="year">The year to check.</param>
    /// <returns>A task that returns the maximum consecutive free days.</returns>
    Task<int> GetMaximumFreeDays(CountryCode country, int year);
}
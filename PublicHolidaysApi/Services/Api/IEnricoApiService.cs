using PublicHolidaysApi.Models;
using PublicHolidaysApi.Models.Enrico;

namespace PublicHolidaysApi.Services.Api;

/// <summary>
/// Defines methods for interacting with the Enrico API.
/// </summary>
public interface IEnricoApiService
{
    /// <summary>
    /// Retrieves a list of supported countries from the Enrico API.
    /// </summary>
    Task<List<EnricoSupportedCountriesDto>> GetSupportedCountriesAsync();

    /// <summary>
    /// Retrieves a list of holidays for a specified year and country.
    /// </summary>
    /// <param name="country">The country code.</param>
    /// <param name="year">The year for which to retrieve holidays.</param>
    Task<List<EnricoHolidaysDto>> GetHolidaysForYearAsync(CountryCode country, int year);

    /// <summary>
    /// Retrieves the workday status for a specified country and date.
    /// </summary>
    /// <param name="country">The country code.</param>
    /// <param name="date">The date for which to retrieve the workday status.</param>
    Task<EnricoWorkDayStatusDto> GetWorkDayStatusAsync(CountryCode country, DateOnly date);

    /// <summary>
    /// Retrieves the public holiday status for a specified country and date.
    /// </summary>
    /// <param name="country">The country code.</param>
    /// <param name="date">The date for which to retrieve the public holiday status.</param>
    Task<EnricoPublicHolidayStatusDto> GetPublicHolidayStatusAsync(CountryCode country, DateOnly date);

    /// <summary>
    /// Retrieves the next workday after a specified date for a given country.
    /// </summary>
    /// <param name="country">The country code.</param>
    /// <param name="date">The starting date.</param>
    /// <param name="deltaDays">Optional number of days to add to the starting date.</param>
    Task<DateWithDayOfWeekDto> GetNextWorkDayAsync(CountryCode country, DateOnly date, int? deltaDays = null);
}
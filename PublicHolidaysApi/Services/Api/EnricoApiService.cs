using PublicHolidaysApi.Helpers;
using PublicHolidaysApi.Models;
using PublicHolidaysApi.Models.Enrico;

namespace PublicHolidaysApi.Services.Api;

/// <inheritdoc cref="IEnricoApiService" />
public class EnricoApiService : ApiServiceBase, IEnricoApiService
{
    private const string GetSupportedCountriesEndpoint = "/getSupportedCountries";
    private const string GetHolidaysForYear = "/getHolidaysForYear/";
    private const string GetNextWorkDay = "/getNextWorkDay/";
    private const string IsWorkDayEndpoint = "/isWorkDay/";
    private const string IsPublicHolidayEndpoint = "/isPublicHoliday/";

    /// ctor
    public EnricoApiService(HttpClient httpClient) : base(httpClient)
    {
    }

    /// <inheritdoc/>
    public async Task<List<EnricoSupportedCountriesDto>> GetSupportedCountriesAsync()
    {
        return await GetResponseAsync<List<EnricoSupportedCountriesDto>>(GetSupportedCountriesEndpoint);
    }

    /// <inheritdoc/>
    public async Task<List<EnricoHolidaysDto>> GetHolidaysForYearAsync(CountryCode country, int year)
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(year), year.ToString() },
            { nameof(country), country.Value }
        };
        
        return await GetResponseAsync<List<EnricoHolidaysDto>>(GetHolidaysForYear + parameters.ToQueryString());
    }

    /// <inheritdoc/>
    public async Task<EnricoWorkDayStatusDto> GetWorkDayStatusAsync(CountryCode country, DateOnly date)
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(date), date.ToString("yyyy-MM-dd") },
            { nameof(country), country.Value }
        };
        
        return await GetResponseAsync<EnricoWorkDayStatusDto>(IsWorkDayEndpoint + parameters.ToQueryString());
    }

    /// <inheritdoc/>
    public async Task<EnricoPublicHolidayStatusDto> GetPublicHolidayStatusAsync(CountryCode country, DateOnly date)
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(date), date.ToString("yyyy-MM-dd") },
            { nameof(country), country.Value }
        };
        
        return await GetResponseAsync<EnricoPublicHolidayStatusDto>(IsPublicHolidayEndpoint + parameters.ToQueryString());
    }

    /// <inheritdoc/>
    public async Task<DateWithDayOfWeekDto> GetNextWorkDayAsync(CountryCode country, DateOnly date, int? deltaDays = null)
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(date), date.ToString("yyyy-MM-dd") },
            { nameof(country), country.Value },
        };
        if (deltaDays.HasValue)
        {
            parameters.Add(nameof(deltaDays), deltaDays.Value.ToString());
        }

        return await GetResponseAsync<DateWithDayOfWeekDto>(GetNextWorkDay + parameters.ToQueryString());
    }
}
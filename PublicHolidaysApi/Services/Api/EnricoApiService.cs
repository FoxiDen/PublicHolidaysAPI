using PublicHolidaysApi.Helpers;
using PublicHolidaysApi.Models;

namespace PublicHolidaysApi.Services.Api;

public class EnricoApiService : ApiServiceBase, IEnricoApiService
{
    private const string GetSupportedCountriesEndpoint = "/getSupportedCountries";
    private const string GetHolidaysForYear = "/getHolidaysForYear/";
    private const string GetNextWorkDay = "/getNextWorkDay/";
    private const string IsWorkDayEndpoint = "/isWorkDay/";
    private const string IsPublicHolidayEndpoint = "/isPublicHoliday/";

    public EnricoApiService(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<List<EnricoSupportedCountriesDto>> GetSupportedCountriesAsync()
    {
        return await GetResponseAsync<List<EnricoSupportedCountriesDto>>(GetSupportedCountriesEndpoint);
    }

    public async Task<List<EnricoHolidaysDto>> GetHolidaysForYearAsync(CountryCode country, int year)
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(year), year.ToString() },
            { nameof(country), country.Value }
        };
        
        return await GetResponseAsync<List<EnricoHolidaysDto>>(GetHolidaysForYear + parameters.ToQueryString());
    }

    public async Task<EnricoWorkDayStatusDto> GetWorkDayStatusAsync(string queryString)
    {
        return await GetResponseAsync<EnricoWorkDayStatusDto>(IsWorkDayEndpoint + queryString);
    }

    public async Task<EnricoPublicHolidayStatusDto> GetPublicHolidayStatusAsync(string queryString)
    {
        return await GetResponseAsync<EnricoPublicHolidayStatusDto>(IsPublicHolidayEndpoint + queryString);
    }

    public async Task<DateWithDayOfWeekDto> GetNextWorkDayAsync(CountryCode country, DateTime date, int? deltaDays = null)
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
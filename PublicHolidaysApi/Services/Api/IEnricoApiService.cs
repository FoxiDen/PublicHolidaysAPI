using PublicHolidaysApi.Models;

namespace PublicHolidaysApi.Services.Api;

public interface IEnricoApiService
{
    Task<List<EnricoSupportedCountriesDto>> GetSupportedCountriesAsync();
    Task<List<EnricoHolidaysDto>> GetHolidaysForYearAsync(CountryCode country, int year);
    Task<EnricoWorkDayStatusDto> GetWorkDayStatusAsync(string queryString);
    Task<EnricoPublicHolidayStatusDto> GetPublicHolidayStatusAsync(string queryString);
    Task<DateWithDayOfWeekDto> GetNextWorkDayAsync(CountryCode country, DateTime date, int? deltaDays = null);
}
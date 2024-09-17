using PublicHolidaysApi.Enums;
using PublicHolidaysApi.Helpers;
using PublicHolidaysApi.Models;
using PublicHolidaysApi.Services.Api;

namespace PublicHolidaysApi.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly IEnricoApiService _enricoApiService;

        public HolidayService(IEnricoApiService enricoApiService)
        {
            _enricoApiService = enricoApiService;
        }

        public async Task<SupportedCountriesCodesDto> GetSupportedCountriesAsync()
        {
            var supportedCountries = await _enricoApiService.GetSupportedCountriesAsync();
            return MappingHelper.MapToSupportedCountryCodeList(supportedCountries);
        }

        public async Task<GroupedHolidaysDto> GetHolidaysAsync(CountryCode country, int year)
        {
            var holidaysForYear = await _enricoApiService.GetHolidaysForYearAsync(country, year);
            return MappingHelper.MapToGroupedHolidaysByMonth(holidaysForYear);
        }

        public async Task<DayStatus> GetSpecificDayStatusAsync(CountryCode country, DateTime date)
        {
            var parameters = new Dictionary<string, string>
            {
                { nameof(date), date.ToString("yyyy-MM-dd") },
                { nameof(country), country.Value }
            };
        
            var queryString = parameters.ToQueryString();
            return await DetermineDayStatusAsync(queryString);
        }
        
        public async Task<int> GetMaximumFreeDays(CountryCode country, int year)
        {
            var holidays = await _enricoApiService.GetHolidaysForYearAsync(country, year);

            var freeDays = new HashSet<DateTime>();
            foreach (var holiday in holidays)
            {
                var date = new DateTime(holiday.Date.Year, holiday.Date.Month, holiday.Date.Day);
                freeDays.Add(date);
            }

            int maxFreeDays = 0;
            
            foreach (var day in freeDays)
            {
                var prevWorkDayDto = await _enricoApiService.GetNextWorkDayAsync(country, day, -1);
                var nextWorkDayDto = await _enricoApiService.GetNextWorkDayAsync(country, day, 1);

                var prevWorkDay = new DateTime(prevWorkDayDto.Year, prevWorkDayDto.Month, prevWorkDayDto.Day);
                var nextWorkDay = new DateTime(nextWorkDayDto.Year, nextWorkDayDto.Month, nextWorkDayDto.Day);
                
                if (prevWorkDay.Year != year)
                {
                    prevWorkDay = new DateTime(year, 1, 1);
                }
                if (nextWorkDay.Year != year)
                {
                    nextWorkDay = new DateTime(year, 12, 31);
                }

                var currentFreeDaysCount = (nextWorkDay - prevWorkDay).Days - 1;
                maxFreeDays = Math.Max(currentFreeDaysCount, maxFreeDays);
            }

            return maxFreeDays;
        }
    
        private async Task<DayStatus> DetermineDayStatusAsync(string queryString)
        {
            if (await _enricoApiService.GetWorkDayStatusAsync(queryString) is { IsWorkDay: true })
            {
                return DayStatus.WorkDay;
            }
        
            if (await _enricoApiService.GetPublicHolidayStatusAsync(queryString) is { IsPublicHoliday: true })
            {
                return DayStatus.PublicHoliday;
            }

            return DayStatus.FreeDay;
        }
    }
}
using PublicHolidaysApi.Helpers;
using PublicHolidaysApi.Models;
using PublicHolidaysApi.Services.Api;
using PublicHolidaysApi.Enums;
using PublicHolidaysApi.Models.Database;
using PublicHolidaysApi.Services.Database;

namespace PublicHolidaysApi.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly IEnricoApiService _enricoApiService;
        private readonly IDatabaseService _databaseService;

        public HolidayService(IEnricoApiService enricoApiService, IDatabaseService databaseService)
        {
            _enricoApiService = enricoApiService;
            _databaseService = databaseService;
        }

        public async Task<SupportedCountriesDto> GetSupportedCountriesAsync()
        {
            var existingCountries = await _databaseService.GetSupportedCountriesAsync();
            if (existingCountries.Any())
            {
                return new SupportedCountriesDto { SupportedCountries = existingCountries };
            }
            
            var countriesFromApi = await _enricoApiService.GetSupportedCountriesAsync();
            var newCountriesDto = MappingHelper.ToSupportedCountriesDto(countriesFromApi);
            await _databaseService.AddSupportedCountriesAsync(newCountriesDto.SupportedCountries);
            
            return newCountriesDto;
        }

        public async Task<GroupedHolidaysDto> GetHolidaysAsync(CountryCode countryCode, int year)
        {
            var existingHolidays = await _databaseService.GetHolidaysAsync(countryCode, year);
            if (existingHolidays.Any())
            {
                return MappingHelper.ToGroupedHolidaysDto(existingHolidays);
            }
            
            var holidaysForYearFromApi = await _enricoApiService.GetHolidaysForYearAsync(countryCode, year);
            await _databaseService.AddHolidaysAsync(MappingHelper.ToHolidayEntityList(holidaysForYearFromApi, countryCode.Value));
            
            return MappingHelper.ToGroupedHolidaysDto(holidaysForYearFromApi);
        }

        public async Task<DayStatus> GetSpecificDayStatusAsync(CountryCode country, DateOnly date)
        {
            var existingDayStatus = await _databaseService.GetDayStatusAsync(country, date);
            if (existingDayStatus != null)
            {
                return (DayStatus)Enum.Parse(typeof(DayStatus), existingDayStatus.Status);
            }
            
            var parameters = new Dictionary<string, string>
            {
                { nameof(date), date.ToString("yyyy-MM-dd") },
                { nameof(country), country.Value }
            };
            var queryString = parameters.ToQueryString();
            
            var dayStatusFromApi = await DetermineDayStatusAsync(queryString);
            await _databaseService.AddDayStatusAsync(new DayStatusEntity { CountryCode = country.Value, Date = date, Status = dayStatusFromApi.ToString() });
            
            return dayStatusFromApi;
        }
        
        public async Task<int> GetMaximumFreeDays(CountryCode country, int year)
        {
            var existingMaximumFreeDays = await _databaseService.GetMaxConsecutiveFreeDaysAsync(country, year);
            if (existingMaximumFreeDays != null)
            {
                return existingMaximumFreeDays.MaxConsecutiveDays;
            }
            
            var maxConsecutiveFreeDays = await GetMaxConsecutiveFreeDaysAsync(country, year);
            await _databaseService.AddMaxConsecutiveFreeDaysAsync(
                new MaxConsecutiveFreeDaysEntity { CountryCode = country.Value, MaxConsecutiveDays = maxConsecutiveFreeDays, Year = year });

            return maxConsecutiveFreeDays;
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
        
        private async Task<int> GetMaxConsecutiveFreeDaysAsync(CountryCode country, int year)
        {
            var holidaysFromApi = await _enricoApiService.GetHolidaysForYearAsync(country, year);

            var freeDays = new HashSet<DateOnly>();
            foreach (var holiday in holidaysFromApi)
            {
                var date = new DateOnly(holiday.Date.Year, holiday.Date.Month, holiday.Date.Day);
                freeDays.Add(date);
            }
            
            int maxConsecutiveFreeDays = 0;
    
            foreach (var day in freeDays)
            {
                var prevWorkDayDto = await _enricoApiService.GetNextWorkDayAsync(country, day, -1);
                var nextWorkDayDto = await _enricoApiService.GetNextWorkDayAsync(country, day, 1);

                var prevWorkDay = new DateOnly(prevWorkDayDto.Year, prevWorkDayDto.Month, prevWorkDayDto.Day);
                var nextWorkDay = new DateOnly(nextWorkDayDto.Year, nextWorkDayDto.Month, nextWorkDayDto.Day);

                if (prevWorkDay.Year != year)
                {
                    prevWorkDay = new DateOnly(year, 1, 1);
                }
                if (nextWorkDay.Year != year)
                {
                    nextWorkDay = new DateOnly(year, 12, 31);
                }

                var currentFreeDaysCount = nextWorkDay.Day - prevWorkDay.Day - 1;
                maxConsecutiveFreeDays = Math.Max(currentFreeDaysCount, maxConsecutiveFreeDays);
            }

            return maxConsecutiveFreeDays;
        }
    }
}
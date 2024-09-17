using Microsoft.EntityFrameworkCore;
using PublicHolidaysApi.Data;
using PublicHolidaysApi.Models;
using PublicHolidaysApi.Models.Database;

namespace PublicHolidaysApi.Services.Database;

public class DatabaseService : IDatabaseService
{
private readonly ApplicationDbContext _context;

        public DatabaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SupportedCountryEntity>> GetSupportedCountriesAsync()
        {
            return await _context.SupportedCountries.AsNoTracking().ToListAsync();
        }

        public async Task<List<HolidayEntity>> GetHolidaysAsync(CountryCode countryCode, int year)
        {
            return await _context.Holidays
                .Where(x => x.CountryCode == countryCode.Value && x.Date.Year == year)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<DayStatusEntity?> GetDayStatusAsync(CountryCode country, DateOnly date)
        {
            return await _context.DayStatuses
                .Where(x => x.CountryCode == country.Value && x.Date == date)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }
    
        public async Task<MaxConsecutiveFreeDaysEntity?> GetMaxConsecutiveFreeDaysAsync(CountryCode country, int year)
        {
            return await _context.MaxConsecutiveFreeDays
                .Where(x => x.CountryCode == country.Value && x.Year == year)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }
        
        public async Task AddSupportedCountriesAsync(IEnumerable<SupportedCountryEntity> countries)
        {
            await AddToDbSetAsync(_context.SupportedCountries, countries);
        }
        
        public async Task AddHolidaysAsync(IEnumerable<HolidayEntity> holidays)
        {
            await AddToDbSetAsync(_context.Holidays, holidays);
        }
        
        public async Task AddDayStatusAsync(DayStatusEntity dayStatus)
        {
            await AddToDbSetAsync(_context.DayStatuses, dayStatus);
        }
        
        public async Task AddMaxConsecutiveFreeDaysAsync(MaxConsecutiveFreeDaysEntity maxFreeDays)
        {
            await AddToDbSetAsync(_context.MaxConsecutiveFreeDays, maxFreeDays);
        }

        private async Task AddToDbSetAsync<TEntity>(DbSet<TEntity> dbSet, object data) where TEntity : class
        {
            switch (data)
            {
                case IEnumerable<TEntity> entities:
                    await dbSet.AddRangeAsync(entities);
                    break;
                case TEntity entity:
                    await dbSet.AddAsync(entity);
                    break;
                default:
                    throw new ArgumentException("The provided data object is not of correct type.");
            }

            await _context.SaveChangesAsync();
        }    
}
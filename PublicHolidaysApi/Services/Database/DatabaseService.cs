using Microsoft.EntityFrameworkCore;
using PublicHolidaysApi.Data;
using PublicHolidaysApi.Models;
using PublicHolidaysApi.Models.Database;

namespace PublicHolidaysApi.Services.Database;

/// <inheritdoc/>
public class DatabaseService : IDatabaseService
{
    private readonly ApplicationDbContext _context;

    /// ctor
    public DatabaseService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<List<SupportedCountryEntity>> GetSupportedCountriesAsync()
    {
        return await _context.SupportedCountries.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<HolidayEntity>> GetHolidaysAsync(CountryCode countryCode, int year)
    {
        return await _context.Holidays
            .Where(x => x.CountryCode == countryCode.Value && x.Date.Year == year)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<DayStatusEntity?> GetDayStatusAsync(CountryCode country, DateOnly date)
    {
        return await _context.DayStatuses
            .Where(x => x.CountryCode == country.Value && x.Date == date)
            .AsNoTracking()
            .SingleOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<MaxConsecutiveFreeDaysEntity?> GetMaxConsecutiveFreeDaysAsync(CountryCode country, int year)
    {
        return await _context.MaxConsecutiveFreeDays
            .Where(x => x.CountryCode == country.Value && x.Year == year)
            .AsNoTracking()
            .SingleOrDefaultAsync();
    }
    
    /// <inheritdoc/>
    public async Task AddSupportedCountriesAsync(IEnumerable<SupportedCountryEntity> countries)
    {
        await AddToDbSetAsync(_context.SupportedCountries, countries);
    }
    
    /// <inheritdoc/>
    public async Task AddHolidaysAsync(IEnumerable<HolidayEntity> holidays)
    {
        await AddToDbSetAsync(_context.Holidays, holidays);
    }
    
    /// <inheritdoc/>
    public async Task AddDayStatusAsync(DayStatusEntity dayStatus)
    {
        await AddToDbSetAsync(_context.DayStatuses, dayStatus);
    }
    
    /// <inheritdoc/>
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
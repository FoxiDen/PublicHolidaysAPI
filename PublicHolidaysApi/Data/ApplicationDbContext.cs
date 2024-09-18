using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PublicHolidaysApi.Models.Database;

namespace PublicHolidaysApi.Data;

/// <summary>
/// Database context for the Public Holidays API.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Represents the supported countries table in the database.
    /// </summary>
    public DbSet<SupportedCountryEntity> SupportedCountries { get; set; }
        
    /// <summary>
    /// Represents the day statuses table in the database.
    /// </summary>
    public DbSet<DayStatusEntity> DayStatuses { get; set; }
        
    /// <summary>
    /// Represents the maximum consecutive free days table in the database.
    /// </summary>
    public DbSet<MaxConsecutiveFreeDaysEntity> MaxConsecutiveFreeDays { get; set; }
        
    /// <summary>
    /// Represents the holidays table in the database.
    /// </summary>
    public DbSet<HolidayEntity> Holidays { get; set; }
    
    /// ctor
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    /// <summary>
    /// Configures the model and relationships for the database context.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SupportedCountryEntity>()
            .HasKey(x => x.CountryCode);

        modelBuilder.Entity<DayStatusEntity>()
            .HasKey(x => new { x.CountryCode, x.Date });

        modelBuilder.Entity<HolidayEntity>()
            .HasKey(x => new { x.CountryCode, x.Date });
        
        modelBuilder.Entity<HolidayEntity>()
            .Property(x=>x.LocalizedNames)
            .HasConversion(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions(JsonSerializerDefaults.General)),
                v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, new JsonSerializerOptions(JsonSerializerDefaults.General))!);
        
        modelBuilder.Entity<MaxConsecutiveFreeDaysEntity>()
            .HasKey(x => new { x.CountryCode, x.Year });
    }
}
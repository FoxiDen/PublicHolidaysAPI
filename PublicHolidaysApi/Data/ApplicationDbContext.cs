using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PublicHolidaysApi.Models;
using PublicHolidaysApi.Models.Database;

namespace PublicHolidaysApi.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<SupportedCountryEntity> SupportedCountries { get; set; }
    public DbSet<DayStatusEntity> DayStatuses { get; set; }
    public DbSet<MaxConsecutiveFreeDaysEntity> MaxConsecutiveFreeDays { get; set; }
    public DbSet<HolidayEntity> Holidays { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
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
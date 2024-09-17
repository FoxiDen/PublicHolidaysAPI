using System.ComponentModel.DataAnnotations.Schema;

namespace PublicHolidaysApi.Models.Database;

public class HolidayEntity
{
    public required string CountryCode { get; set; }
    public required DateOnly Date { get; set; }

    [Column("localized_names")]
    public required Dictionary<string, string> LocalizedNames { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicHolidaysApi.Models.Database;

/// <summary>
/// Entity representing a holiday in a specific country.
/// </summary>
public class HolidayEntity
{
    /// <summary>
    /// Country code for the holiday.
    /// </summary>
    public required string CountryCode { get; set; }

    /// <summary>
    /// Date of the holiday.
    /// </summary>
    public required DateOnly Date { get; set; }

    /// <summary>
    /// Localized names of the holiday.
    /// </summary>
    [Column("localized_names")]
    public required Dictionary<string, string> LocalizedNames { get; set; }
}
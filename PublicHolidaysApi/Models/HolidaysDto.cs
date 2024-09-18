namespace PublicHolidaysApi.Models;

/// <summary>
/// Data transfer object of simplified holiday entry with the day and localized names.
/// </summary>
public record HolidaysDto
{
    /// <summary>
    /// The day of the holiday.
    /// </summary>
    public required int Day { get; init; }
    
    /// <summary>
    /// Localized names for the holiday.
    /// </summary>
    public required List<LocalizedNamesDto> LocalizedNames { get; init; }
}
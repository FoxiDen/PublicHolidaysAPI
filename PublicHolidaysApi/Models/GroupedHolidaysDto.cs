namespace PublicHolidaysApi.Models;

/// <summary>
/// Data transfer object for holidays grouped by month.
/// </summary>
public record GroupedHolidaysDto
{
    /// <summary>
    /// A dictionary where the key is the month name and the value is a list of holidays for that month.
    /// </summary>
    public required Dictionary<string, List<HolidaysDto>> HolidaysByMonth { get; init; }
}
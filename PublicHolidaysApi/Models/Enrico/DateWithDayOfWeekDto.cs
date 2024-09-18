namespace PublicHolidaysApi.Models.Enrico;

/// <summary>
/// Data transfer object representing a specific date along with the day of the week.
/// </summary>
public record DateWithDayOfWeekDto : DateBaseDto
{
    /// <summary>
    /// Day of the week (e.g., 7 for Sunday, 1 for Monday).
    /// </summary>
    public required int DayOfWeek { get; init; }
}
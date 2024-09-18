namespace PublicHolidaysApi.Models.Enrico;

/// <summary>
/// Data transfer object representing whether day is public holiday from the Enrico API.
/// </summary>
public class EnricoPublicHolidayStatusDto
{
    /// <summary>
    /// Indicates whether the date is a public holiday.
    /// </summary>
    public required bool IsPublicHoliday { get; init; }
}
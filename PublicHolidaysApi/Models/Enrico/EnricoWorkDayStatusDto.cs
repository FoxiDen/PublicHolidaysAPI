namespace PublicHolidaysApi.Models.Enrico;

/// <summary>
/// Data transfer object representing whether specific day is workday from the Enrico API.
/// </summary>
public record EnricoWorkDayStatusDto
{
    /// <summary>
    /// Indicates whether the date is a workday.
    /// </summary>
    public required bool IsWorkDay { get; init; }
}
namespace PublicHolidaysApi.Models.Database;

/// <summary>
/// Entity representing the status of a specific day in a given country.
/// </summary>
public class DayStatusEntity
{
    /// <summary>
    /// Country code associated with the day status.
    /// </summary>
    public required string CountryCode { get; set; }

    /// <summary>
    /// Date of the status.
    /// </summary>
    public required DateOnly Date { get; set; }

    /// <summary>
    /// Status of the day (e.g., FreeDay, WorkDay, PublicHoliday).
    /// </summary>
    public required string Status { get; set; }
}
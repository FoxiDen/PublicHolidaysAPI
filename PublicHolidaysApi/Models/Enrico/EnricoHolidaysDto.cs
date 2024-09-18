namespace PublicHolidaysApi.Models.Enrico;

/// <summary>
/// Data transfer object representing a holiday retrieved from the Enrico API.
/// </summary>
public record EnricoHolidaysDto
{
    /// <summary>
    /// Date and day of the week of the holiday.
    /// </summary>
    public required DateWithDayOfWeekDto Date { get; init; }

    /// <summary>
    /// Localized names of the holiday.
    /// </summary>
    public required List<LocalizedNamesDto> Name { get; init; }

    /// <summary>
    /// Type of the holiday.
    /// </summary>
    public required string HolidayType { get; init; }
}
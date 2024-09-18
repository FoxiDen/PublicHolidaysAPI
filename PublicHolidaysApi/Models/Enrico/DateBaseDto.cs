namespace PublicHolidaysApi.Models.Enrico;

/// <summary>
/// Data transfer object representing a specific date.
/// </summary>
public record DateBaseDto
{
    /// <summary>
    /// Day of the date.
    /// </summary>
    public required int Day { get; init; }

    /// <summary>
    /// Month of the date.
    /// </summary>
    public required int Month { get; init; }

    /// <summary>
    /// Year of the date.
    /// </summary>
    public required int Year { get; init; }
}


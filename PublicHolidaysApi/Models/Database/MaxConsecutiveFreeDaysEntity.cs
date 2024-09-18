namespace PublicHolidaysApi.Models.Database;

/// <summary>
/// Entity representing the maximum number of consecutive free days in a given year for a specific country.
/// </summary>
public class MaxConsecutiveFreeDaysEntity
{
    /// <summary>
    /// Country code associated with the data.
    /// </summary>
    public required string CountryCode { get; set; }

    /// <summary>
    /// Year for which the maximum consecutive free days are recorded.
    /// </summary>
    public required int Year { get; set; }

    /// <summary>
    /// Maximum number of consecutive free days in the given year.
    /// </summary>
    public required int MaxConsecutiveDays { get; set; }
}
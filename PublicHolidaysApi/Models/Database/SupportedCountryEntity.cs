namespace PublicHolidaysApi.Models.Database;

/// <summary>
/// Entity representing a supported country.
/// </summary>
public class SupportedCountryEntity
{
    /// <summary>
    /// Country code.
    /// </summary>
    public required string CountryCode { get; set; }

    /// <summary>
    /// Country name.
    /// </summary>
    public required string CountryName { get; set; }
}
namespace PublicHolidaysApi.Models.Enrico;

/// <summary>
/// Data transfer object representing supported country details from the Enrico API.
/// </summary>
public record EnricoSupportedCountriesDto
{
    /// <summary>
    /// Country code.
    /// </summary>
    public required string CountryCode { get; init; }

    /// <summary>
    /// List of regions within the country.
    /// </summary>
    public required List<string> Regions { get; init; }

    /// <summary>
    /// List of holiday types supported.
    /// </summary>
    public required List<string> HolidayTypes { get; init; }

    /// <summary>
    /// Full name of the country.
    /// </summary>
    public required string FullName { get; init; }

    /// <summary>
    /// Start date for the supported holidays.
    /// </summary>
    public required DateBaseDto FromDate { get; init; }

    /// <summary>
    /// End date for the supported holidays.
    /// </summary>
    public required DateBaseDto ToDate { get; init; }
}
using PublicHolidaysApi.Models.Database;

namespace PublicHolidaysApi.Models;

/// <summary>
/// Record of data transfer object containing a list of supported countries.
/// </summary>
public record SupportedCountriesDto
{
    /// <summary>
    /// The list of supported countries.
    /// </summary>
    public required List<SupportedCountryEntity> SupportedCountries { get; init; }
}
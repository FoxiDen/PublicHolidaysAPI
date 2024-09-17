using PublicHolidaysApi.Models.Database;

namespace PublicHolidaysApi.Models;

public record SupportedCountriesDto
{
    public required List<SupportedCountryEntity> SupportedCountries { get; init; }
}
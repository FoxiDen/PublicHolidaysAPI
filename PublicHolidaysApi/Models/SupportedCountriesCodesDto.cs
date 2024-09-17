namespace PublicHolidaysApi.Models;

public record SupportedCountriesCodesDto
{
    public required List<SupportedCountryCode> SupportedCountries { get; init; }
}
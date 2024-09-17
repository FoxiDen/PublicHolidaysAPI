namespace PublicHolidaysApi.Models;

public record SupportedCountryCode
{
    public required string Name { get; init;}
    public required CountryCode Code { get; init; }
}
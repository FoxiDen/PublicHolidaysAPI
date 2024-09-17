namespace PublicHolidaysApi.Models.Enrico;

public record EnricoSupportedCountriesDto
{
    public required string CountryCode { get; init; }
    public required List<string> Regions { get; init; }
    public required List<string> HolidayTypes { get; init; }
    public required string FullName { get; init; }
    public required DateBaseDto FromDate { get; init; }
    public required DateBaseDto ToDate { get; init; }
}
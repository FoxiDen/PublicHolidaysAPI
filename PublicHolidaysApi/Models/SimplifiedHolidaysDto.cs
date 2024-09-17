namespace PublicHolidaysApi.Models;

public record SimplifiedHolidaysDto
{
    public required int Day { get; init; }
    public required List<LocalizedNamesDto> LocalizedNames { get; init; }
}
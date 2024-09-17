namespace PublicHolidaysApi.Models;

public record SimplifiedHolidaysDto
{
    public required int Day { get; init; }
    public required List<LocalizedTextDto> Name { get; init; }
}
namespace PublicHolidaysApi.Models;

public record GroupedHolidaysDto
{
    public required Dictionary<string, List<SimplifiedHolidaysDto>> HolidaysByMonth { get; init; }
}
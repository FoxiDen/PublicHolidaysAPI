namespace PublicHolidaysApi.Models;

public record EnricoHolidaysDto
{
    public required DateWithDayOfWeekDto Date { get; init; }
    public required List<LocalizedTextDto> Name { get; init; }
    public required string HolidayType { get; init; }
}
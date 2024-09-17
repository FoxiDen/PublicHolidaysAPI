namespace PublicHolidaysApi.Models.Enrico;

public record EnricoHolidaysDto
{
    public required DateWithDayOfWeekDto Date { get; init; }
    public required List<LocalizedNamesDto> Name { get; init; }
    public required string HolidayType { get; init; }
}
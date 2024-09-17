namespace PublicHolidaysApi.Models.Enrico;

public record DateWithDayOfWeekDto : DateBaseDto
{
    public required int DayOfWeek { get; init; }
}
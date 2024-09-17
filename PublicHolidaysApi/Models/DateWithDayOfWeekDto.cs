namespace PublicHolidaysApi.Models;

public record DateWithDayOfWeekDto : DateBaseDto
{
    public required int DayOfWeek { get; init; }
}
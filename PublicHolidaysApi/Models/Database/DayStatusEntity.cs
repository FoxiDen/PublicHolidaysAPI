namespace PublicHolidaysApi.Models.Database;

public class DayStatusEntity
{
    public required string CountryCode { get; set; }
    public required DateOnly Date { get; set; }
    public required string Status { get; set; }
}
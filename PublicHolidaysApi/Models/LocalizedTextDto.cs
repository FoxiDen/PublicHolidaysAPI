namespace PublicHolidaysApi.Models;

public record LocalizedTextDto
{
    public required string Lang { get; init; }
    public required string Text { get; init; }
}
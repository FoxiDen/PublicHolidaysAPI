namespace PublicHolidaysApi.Models;

public record LocalizedNamesDto
{
    public required string Lang { get; init; }
    public required string Text { get; init; }
}
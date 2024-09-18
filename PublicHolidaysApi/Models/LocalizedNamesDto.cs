namespace PublicHolidaysApi.Models;

/// <summary>
/// Data transfer object of localized name and its language.
/// </summary>
public record LocalizedNamesDto
{
    /// <summary>
    /// The language code of the localized name.
    /// </summary>
    public required string Lang { get; init; }
    
    /// <summary>
    /// The localized name text.
    /// </summary>
    public required string Text { get; init; }
}
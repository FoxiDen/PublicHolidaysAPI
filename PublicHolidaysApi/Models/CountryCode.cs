using Vogen;

namespace PublicHolidaysApi.Models;

/// <summary>
/// Country code representation used for identifying countries in requests. Format "xxx", must be letters.
/// </summary>
[ValueObject(typeof(string))]
public partial struct CountryCode
{
    /// <summary>
    /// Validates the format of the country code.
    /// </summary>
    public static Validation Validate(string value)
    {
        if (value.Length != 3)
        {
            return Validation.Invalid("CountryCode must be made out of 3 characters.");
        }
        
        if (!value.All(char.IsLetter))
        {
            return Validation.Invalid("CountryCode must only contain letters.");
        }

        return Validation.Ok;
    }
}
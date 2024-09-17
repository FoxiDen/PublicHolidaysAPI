using Vogen;

namespace PublicHolidaysApi.Models;

[ValueObject(typeof(string))]
public partial struct CountryCode
{
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
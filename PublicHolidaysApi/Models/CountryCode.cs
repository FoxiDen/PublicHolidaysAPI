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
        return Validation.Ok;
    }
}
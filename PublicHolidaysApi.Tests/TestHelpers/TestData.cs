using PublicHolidaysApi.Models;
using PublicHolidaysApi.Models.Enrico;

namespace PublicHolidaysApi.Tests.TestHelpers;

public static class TestData
{
    public static readonly DateOnly NewYearDate = new(2024, 1, 1);
    public static readonly DateOnly ValentinesDayDate = new(2024, 2, 14);
    public static readonly DateOnly ChineseNewYearDate = new(2024, 3, 10);
    public static readonly DateOnly CarnivalDate = new(2024, 4, 12);
    
    public static readonly LocalizedNamesDto NewYearEnglish = new() { Lang = "en", Text = "New Year" };
    public static readonly LocalizedNamesDto NewYearLithuanian = new()  { Lang = "lt", Text = "Naujieji metai" };
    public static readonly LocalizedNamesDto ValentinesDayEnglish = new()  { Lang = "en", Text = "Valentine's Day" };
    public static readonly LocalizedNamesDto ValentinesDayLithuanian = new()  { Lang = "lt", Text = "Šv. Valentino diena" };

    public static readonly DateBaseDto AnyDateBaseDto = new() { Year = 1, Month = 1, Day = 1 };
    
    public static readonly Dictionary<string, string> ChineseNewYearNamesDict = new()
    {
        { "en", "Chinese New Year" },
        { "lt", "Kinų Naujieji Metai" }
    };

    public static readonly Dictionary<string, string> CarnivalNamesDict = new()
    {
        { "en", "Carnival" },
    };
    
    public const string CAN = "CAN";
    public const string LTU = "LTU";

    public const string Canada = "UnitedStatesOfAmerica";
    public const string Lithuania = "Lithuania";
    
    public const string PublicHoliday = "Public";
    public const string Observance = "Observance";
}
using System.Net;

namespace PublicHolidaysApi.Helpers;

public static class DictionaryExtensions
{
    public static string ToQueryString(this Dictionary<string, string> parameters)
    {
        if (parameters.Count == 0)
        {
            return string.Empty;
        }
        
        return "?" + string.Join("&", parameters.Select(p => 
            $"{WebUtility.UrlEncode(p.Key)}={WebUtility.UrlEncode(p.Value)}"));
    }
}
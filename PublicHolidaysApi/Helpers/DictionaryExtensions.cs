using System.Net;
using PublicHolidaysApi.Models;

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

    public static List<LocalizedNamesDto> ToLocalizedNamesDtoList(this Dictionary<string, string> dictionary)
    {
        return dictionary.Select(kv => new LocalizedNamesDto
        {
            Lang = kv.Key,
            Text = kv.Value
        }).ToList();
    }
}
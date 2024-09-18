using System.Net;
using PublicHolidaysApi.Models;

namespace PublicHolidaysApi.Helpers;

/// <summary>
/// Provides extension methods for Dictionary.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Converts a dictionary of query parameters to a query string for url.
    /// </summary>
    public static string ToQueryString(this Dictionary<string, string> parameters)
    {
        if (parameters.Count == 0)
        {
            return string.Empty;
        }
        
        return "?" + string.Join("&", parameters.Select(p => 
            $"{WebUtility.UrlEncode(p.Key)}={WebUtility.UrlEncode(p.Value)}"));
    }

    /// <summary>
    /// Converts a dictionary of string on string to a list of <see cref="LocalizedNamesDto"/>.
    /// </summary>
    public static List<LocalizedNamesDto> ToLocalizedNamesDtoList(this Dictionary<string, string> dictionary)
    {
        return dictionary.Select(kv => new LocalizedNamesDto
        {
            Lang = kv.Key,
            Text = kv.Value
        }).ToList();
    }
}
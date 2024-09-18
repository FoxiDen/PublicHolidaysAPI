using System.Text.Json;

namespace PublicHolidaysApi.Services.Api;

/// <summary>
/// A base class for API services.
/// </summary>
public abstract class ApiServiceBase
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions DeserializationOptions = new()
    {
        PropertyNameCaseInsensitive = true 
    };
    
    /// ctor
    protected ApiServiceBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Sends a GET request to the specified endpoint and deserializes the response.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response into.</typeparam>
    /// <param name="endpoint">The endpoint to send the request to.</param>
    protected async Task<T> GetResponseAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync(_httpClient.BaseAddress + endpoint);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<T>(responseContent, DeserializationOptions);
        if (result is null)
        {
            throw new HttpRequestException("Failed to deserialize data.");
        }

        return result;
    }
}
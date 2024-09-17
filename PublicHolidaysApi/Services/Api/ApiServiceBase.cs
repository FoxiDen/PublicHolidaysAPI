using System.Text.Json;

namespace PublicHolidaysApi.Services;

public abstract class ApiServiceBase
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions DeserializationOptions = new()
    {
        PropertyNameCaseInsensitive = true 
    };
    
    protected ApiServiceBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

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
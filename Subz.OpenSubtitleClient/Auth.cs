using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Serilog;
using Subz.OpenSubtitleClient.Objects;

namespace Subz.OpenSubtitleClient;

public class Auth
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.opensubtitles.com/api/v1";
    
    public Auth()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Subz v1.0"); // TODO: Replace with your app name/version
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public async Task<LoginResponse> LoginAsync(string username, string password, string apiKey)
    {
        var loginRequest = new LoginRequest
        {
            Username = username,
            Password = password
        };

        var content = new StringContent(
            JsonSerializer.Serialize(loginRequest),
            Encoding.UTF8,
            "application/json");

        _httpClient.DefaultRequestHeaders.Add("Api-Key", apiKey);
        var response = await _httpClient.PostAsync($"{BaseUrl}/login", content);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<LoginResponse>(responseContent);
        if (response.IsSuccessStatusCode) 
            return result;
        
        Log.Warning("Failed to login: {StatusCode}", response.StatusCode);
        result.Status = (int)response.StatusCode;

        return result;
    }
}
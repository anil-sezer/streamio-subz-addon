using System.Text.Json.Serialization;

namespace Subz.OpenSubtitleClient.Objects;

// todo: set should be removed and all should be init only
public class LoginResponse
{
    [JsonPropertyName("base_url")]
    public string BaseUrl { get; set; }
    
    [JsonPropertyName("token")]
    public string Token { get; set; }
    
    [JsonPropertyName("status")]
    public int Status { get; set; }
    
    [JsonPropertyName("user")]
    public User User { get; set; }
    
    [JsonPropertyName("message")]
    public string ErrorMessage { get; set; }
}

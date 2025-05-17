using Subz.OpenSubtitleClient.Services.Dtos.Login.SubDtos;

namespace Subz.OpenSubtitleClient.Services.Dtos.Login;

// todo: set should be removed and all should be init only
public class LoginResponse: OsResponseBase
{
    [JsonPropertyName("base_url")]
    public string BaseUrl { get; set; }
    
    [JsonPropertyName("token")]
    public string Token { get; set; }
    
    [JsonPropertyName("status")]
    public int Status { get; set; }
    
    [JsonPropertyName("user")]
    public User User { get; set; }
}

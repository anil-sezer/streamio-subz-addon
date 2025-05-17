namespace Subz.OpenSubtitleClient.Services.Dtos.Login;

public class LoginRequest
{
    [JsonPropertyName("username")]
    public required string Username { get; set; }
    [JsonPropertyName("password")]
    public required string Password { get; set; }
}

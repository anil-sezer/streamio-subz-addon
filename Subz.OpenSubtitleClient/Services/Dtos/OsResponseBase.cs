namespace Subz.OpenSubtitleClient.Services.Dtos;

// todo: set should be removed and all should be init only
public class OsResponseBase
{
    public bool IsSuccessStatusCode { get; set; }
    public int StatusCode { get; set; }
    
    [JsonPropertyName("message")]
    public string ErrorMessage { get; set; }
}
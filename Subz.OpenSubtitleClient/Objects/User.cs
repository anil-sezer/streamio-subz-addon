using System.Text.Json.Serialization;

namespace Subz.OpenSubtitleClient.Objects;

public class User
{
    [JsonPropertyName("allowed_downloads")]
    public int AllowedDownloads { get; set; }
        
    [JsonPropertyName("allowed_translations")]
    public int AllowedTranslations { get; set; }
        
    [JsonPropertyName("level")]
    public string Level { get; set; }
        
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }
        
    [JsonPropertyName("ext_installed")]
    public bool ExtInstalled { get; set; }
        
    [JsonPropertyName("vip")]
    public bool Vip { get; set; }
}
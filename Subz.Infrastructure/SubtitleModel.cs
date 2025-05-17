using System.Text.Json.Serialization;

namespace Subz.Infrastructure;

public class SubtitleModel
{
    [JsonPropertyName("id")]
    public required string NameToDisplay { get; init; }
    
    [JsonPropertyName("lang")]
    public required string Language { get; init; }
    
    [JsonPropertyName("url")]
    public required string DownloadCommand { get; init; }
    // todo: I dont see this at the official object: https://github.com/Stremio/stremio-addon-sdk/blob/master/docs/api/responses/subtitles.md
    // [JsonPropertyName("format")]
    // public required string FileFormat { get; init; }
}
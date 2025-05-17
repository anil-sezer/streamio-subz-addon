namespace Subz.OpenSubtitleClient.Services.Dtos.Download;

public class DownloadRequest
{
    [JsonPropertyName("file_id")]
    public required string FileId { get; init; }
}

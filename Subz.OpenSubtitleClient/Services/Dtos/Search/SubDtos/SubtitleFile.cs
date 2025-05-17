namespace Subz.OpenSubtitleClient.Services.Dtos.Search.SubDtos;

public class SubtitleFile
{
    [JsonPropertyName("file_id")]
    public int FileId { get; set; }

    [JsonPropertyName("cd_number")]
    public int CdNumber { get; set; }

    [JsonPropertyName("file_name")]
    public string FileName { get; set; }
}

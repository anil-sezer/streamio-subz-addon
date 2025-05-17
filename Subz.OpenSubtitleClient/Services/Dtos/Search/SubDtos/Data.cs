namespace Subz.OpenSubtitleClient.Services.Dtos.Search.SubDtos;

public class Data
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("attributes")]
    public SubtitleAttributes Attributes { get; set; }
}

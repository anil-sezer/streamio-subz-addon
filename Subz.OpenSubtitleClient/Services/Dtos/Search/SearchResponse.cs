using Subz.OpenSubtitleClient.Services.Dtos.Search.SubDtos;

namespace Subz.OpenSubtitleClient.Services.Dtos.Search;

// todo: tmdb_id lazım olur mu?
public class SearchResponse: OsResponseBase
{
    [JsonPropertyName("total_pages")]
    public required int total_pages { get; set; }
    
    [JsonPropertyName("total_count")]
    public required int total_count { get; set; }
    
    [JsonPropertyName("per_page")]
    public required int per_page { get; set; }
    
    [JsonPropertyName("page")]
    public required int page { get; set; }
    
    [JsonPropertyName("data")]
    public required List<Data> data { get; set; }
}
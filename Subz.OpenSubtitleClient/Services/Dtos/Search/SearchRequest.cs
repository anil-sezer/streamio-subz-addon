namespace Subz.OpenSubtitleClient.Services.Dtos.Search;

// todo: tmdb_id lazım olur mu?
public class SearchRequest
{
    [JsonPropertyName("imdb_id")]
    public required string ImdbId { get; init; }
    
    [JsonPropertyName("hearing_impaired")]
    public required string HearingImpairedOption { get; init; }
    
    [JsonPropertyName("languages")]
    public required List<string> Languages { get; init; }
    
    // For TV shows
    [JsonPropertyName("season_number")]
    public string? SeasonNumber { get; init; }
    [JsonPropertyName("episode_number")]
    public string? EpisodeNumber { get; init; }
}
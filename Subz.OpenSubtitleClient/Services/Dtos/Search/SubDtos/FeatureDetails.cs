namespace Subz.OpenSubtitleClient.Services.Dtos.Search.SubDtos;

public class FeatureDetails
{
    [JsonPropertyName("feature_id")]
    public int FeatureId { get; set; }

    [JsonPropertyName("feature_type")]
    public string FeatureType { get; set; }

    [JsonPropertyName("year")]
    public int? Year { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("movie_name")]
    public string MovieName { get; set; }

    [JsonPropertyName("imdb_id")]
    public int ImdbId { get; set; }

    [JsonPropertyName("tmdb_id")]
    public int TmdbId { get; set; }
}

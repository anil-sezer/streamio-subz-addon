namespace Subz.OpenSubtitleClient.Services.Dtos.Search.SubDtos;

public class SubtitleAttributes
    {
        [JsonPropertyName("subtitle_id")]
        public string SubtitleId { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("hearing_impaired")]
        public bool HearingImpaired { get; set; }

        [JsonPropertyName("fps")]
        public double Fps { get; set; }

        [JsonPropertyName("ai_translated")]
        public bool AiTranslated { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("release")]
        public string Release { get; set; }

        [JsonPropertyName("feature_details")]
        public FeatureDetails FeatureDetails { get; set; }

        [JsonPropertyName("files")]
        public List<SubtitleFile> Files { get; set; }
    }

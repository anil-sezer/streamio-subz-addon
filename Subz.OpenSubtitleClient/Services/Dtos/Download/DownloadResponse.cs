namespace Subz.OpenSubtitleClient.Services.Dtos.Download;

public class DownloadResponse: OsResponseBase
{
    [JsonPropertyName("link")]
    public string Link { get; set; }

    [JsonPropertyName("file_name")]
    public string FileName { get; set; }

    // todo: Not needed, can be removed
    // [JsonPropertyName("requests")]
    // public int Requests { get; set; }
    //
    // [JsonPropertyName("remaining")]
    // public int Remaining { get; set; }
    //
    // [JsonPropertyName("message")]
    // public string Message { get; set; }
    //
    // [JsonPropertyName("reset_time")]
    // public string ResetTime { get; set; }
    //
    // [JsonPropertyName("reset_time_utc")]
    // public string ResetTimeUtc { get; set; }
    //
    // [JsonPropertyName("uk")]
    // public string Uk { get; set; }
    //
    // [JsonPropertyName("uid")]
    // public int Uid { get; set; }
    //
    // [JsonPropertyName("ts")]
    // public long Ts { get; set; }
}

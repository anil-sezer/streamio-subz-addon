using Subz.OpenSubtitleClient;

namespace Subz.Models;

public class UserProfile
{
    // public required OsCredentials  OsCredentials { get; init; }
    public required OsClient  OsClient { get; init; }

    public required List<string> Languages { get; init; }

    public required string HearingImpairedOption { get; init; }

    public string? DiscordWebhook { get; init; }
    
    public required string InstallAddonUrl { get; init; }
    public required string AppUrl { get; init; }
    public required string UserConfigQueryString { get; init; }
}
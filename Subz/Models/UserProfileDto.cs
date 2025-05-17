namespace Subz.Models;

public class UserProfileDto
{
    public required string WindowUrl {get; init;}

    // OpenSubtitle credentials
    public required string OsUsername {get; init;}
    public required string OsPassword {get; init;}
    public required string OsApiKey {get; init;}
    
    // Options
    public required string HearingImpairedOption {get; init;}
    public required List<string> SelectedLanguages {get; init;}
    
    // Optional
    public string DiscordWebhook {get; init;}
    
}
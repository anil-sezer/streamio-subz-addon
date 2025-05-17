namespace Subz.OpenSubtitleClient;

public class OsClient
{
    public readonly HttpClient HttpClient;
    public readonly string Username;
    public readonly string Password;
    public readonly string ApiKey;
    public readonly string DiscordWebhookUrl; // todo: remove and only use it from UserProfile

    public OsClient(string username, string password, string apiKey, HttpClient httpClient, string discordWebhookUrl)
    {
        Username = username;
        Password = password;
        ApiKey = apiKey;
        HttpClient = httpClient;
        DiscordWebhookUrl = discordWebhookUrl;
    }
}
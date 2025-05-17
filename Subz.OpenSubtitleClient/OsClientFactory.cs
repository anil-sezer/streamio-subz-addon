using System.Net.Http.Headers;
using Subz.Infrastructure.ConstantsAndEnums;
using Subz.Infrastructure.Exceptions;
using Subz.OpenSubtitleClient.Services;

namespace Subz.OpenSubtitleClient;

public static class OsClientFactory
{
    public static async Task<OsClient> CreateAsync(
        string username, 
        string password, 
        string apiKey,
        string discordWebhookUrl = ""
        )
    {
        var httpClient = new HttpClient();
        
        httpClient.DefaultRequestHeaders.Add("User-Agent", Environment.GetEnvironmentVariable(EnvironmentVariableNames.AppUserAgent) ?? "Subz");
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Add("Api-Key", apiKey);
        
        var client = new OsClient(username, password, apiKey, httpClient, discordWebhookUrl);
        
        var result = await client.LoginAsync();

        if (string.IsNullOrEmpty(result.Token))
            throw new InvalidCredentialsException(result.ErrorMessage);
        
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);
        await Task.Delay(500);

        if (!await client.IsTokenValidAsync())
            throw new InvalidCredentialsException("Token is not valid");
        
        return client;
    }
}

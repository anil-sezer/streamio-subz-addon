using System.Text;
using System.Text.Json;
using Serilog;
using Subz.Infrastructure.ConstantsAndEnums;
using Subz.Infrastructure.Services;
using Subz.OpenSubtitleClient.Services.Dtos.Login;

namespace Subz.OpenSubtitleClient.Services;

public static class Auth
{
    public static async Task<bool> IsTokenValidAsync(this OsClient client)
    {
        try
        {
            var response = await client.HttpClient.GetAsync($"{OsConsts.BaseUrl}/infos/user");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Error validating token");
            return false;
        }
    }
    
    public static async Task<LoginResponse> LoginAsync(this OsClient client)
    {
        var httpResponse = await InitiateLogin(client);
        var serializedResponse = await SerializeResponse(httpResponse, client);
        
        if (serializedResponse.IsSuccessStatusCode) 
            return serializedResponse;
        
        serializedResponse.ErrorMessage = $"Login failed with {serializedResponse.Status} code. {Environment.NewLine}Message from Opensubtitles: {serializedResponse.ErrorMessage}" + 
                                (serializedResponse.Status == 403 ? $"{Environment.NewLine}{Environment.NewLine}Tip from addon dev: You are probably using wrong credentials." : "");
        
        Log.Error("Failed to login: {StatusCode}", serializedResponse.Status);

        return serializedResponse;
    }

    private static async Task<HttpResponseMessage> InitiateLogin(OsClient client)
    {
        var loginRequest = new LoginRequest
        {
            Username = client.Username,
            Password = client.Password
        };

        var content = new StringContent(
            JsonSerializer.Serialize(loginRequest),
            Encoding.UTF8,
            "application/json");

        return await client.HttpClient.PostAsync($"{OsConsts.BaseUrl}/login", content);
    }

    private static async Task<LoginResponse> SerializeResponse(HttpResponseMessage response, OsClient client)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        var serializedResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent);

        if (serializedResponse is null)
            throw await HandleInvalidResponseAsync(client);
        
        serializedResponse.IsSuccessStatusCode = response.IsSuccessStatusCode;
        serializedResponse.Status = (int)response.StatusCode;

        return serializedResponse;
    }
    
    private static async Task<Exception> HandleInvalidResponseAsync(this OsClient client)
    {
        var msg = $"Failed to deserialize login response for user {client.Username}, at OpenSubtitles";
        
        Log.Fatal(msg);
        await NotifyViaDiscord.SendNotificationAsync(
            msg,
            client.DiscordWebhookUrl,
            DiscordMessageType.Error,
            "OpenSubtitles Login Error"
        );
            
        return new Exception(msg);
    }
}

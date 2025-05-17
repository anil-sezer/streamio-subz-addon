using System.Text;
using System.Text.Json;
using Serilog;
using Subz.Infrastructure.ConstantsAndEnums;

namespace Subz.Infrastructure.Services;

// todo: I think this is unable to init itself when push comes to shove.
// Connect this to the exception flow 
public static class NotifyViaDiscord
{
    public static async Task<bool> SendNotificationAsync(
        string message, 
        DiscordMessageType messageType = DiscordMessageType.Neutral,
        string? title = null)
    {
        // TODO: GET WEBHOOK URL FROM ASYNCLOCAL
        var webhookUrl = "";
        return await SendNotificationAsync(message, webhookUrl, messageType, title);
    }

    public static async Task<bool> SendNotificationAsync(
        string message, 
        string webhookUrl,
        DiscordMessageType messageType = DiscordMessageType.Neutral,
        string? title = null)
    {
        if (string.IsNullOrEmpty(webhookUrl))
        {
            Log.Information("Discord webhook URL is not set. So we are not sending any notifications to outside.");
            return false;
        }
        
        try
        {
            using var client = new HttpClient();
            
            // Determine color based on message type
            int colorValue = messageType switch
            {
                DiscordMessageType.Success => 0x3cb371,  // Green (#3CB371)
                DiscordMessageType.Error => 0xed4245,    // Red (#ED4245)
                DiscordMessageType.Warning => 0xfee75c,  // Yellow (#FEE75C)
                _ => 0x5865f2                            // Blue (#5865F2) - Discord default
            };

            // Create discord embed with color
            var embed = new
            {
                title = title,
                description = message,
                color = colorValue
            };

            // Create payload with embed
            var payload = new
            {
                embeds = new[] { embed }
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(webhookUrl, content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Log.Error($"Discord API error: {response.StatusCode} - {errorContent}");
                return false;
            }
            
            return true;
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to send Discord notification: {ex.Message}");
            return false;
        }
    }
}

using System.Text;
using System.Text.Json;
using Serilog;
using Subz.Infrastructure.Exceptions;
using Subz.OpenSubtitleClient.Services.Dtos.Download;

namespace Subz.OpenSubtitleClient.Services;

public static class Download
{
    public static async Task<DownloadResponse> DownloadSubtitleAsync(this OsClient client, DownloadRequest request)
    {
        var httpResponse = await InitiateGettingDownloadLinkAsync(client, request);
        var serializedResponse = await SerializeResponseAsync(httpResponse);

        if (serializedResponse.IsSuccessStatusCode)
            return serializedResponse;

        Log.Error("Failed to get download link: {StatusCode}", serializedResponse.StatusCode);
        throw new UnknownErrorException();
    }

    private static async Task<HttpResponseMessage> InitiateGettingDownloadLinkAsync(OsClient client, DownloadRequest request)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        return await client.HttpClient.PostAsync($"{OsConsts.BaseUrl}/download", content);
    }
    
    // todo: make this generic
    private static async Task<DownloadResponse> SerializeResponseAsync(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        var serializedResponse = JsonSerializer.Deserialize<DownloadResponse>(responseContent);
        
        serializedResponse.IsSuccessStatusCode = response.IsSuccessStatusCode;
        serializedResponse.StatusCode = (int)response.StatusCode;

        return serializedResponse;
    }
}

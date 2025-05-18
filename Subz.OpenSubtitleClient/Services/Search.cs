using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Serilog;
using Subz.Infrastructure;
using Subz.Infrastructure.ConstantsAndEnums;
using Subz.Infrastructure.Exceptions;
using Subz.Infrastructure.Services;
using Subz.OpenSubtitleClient.Services.Dtos.Search;

namespace Subz.OpenSubtitleClient.Services;

public static class Search
{
    public static async Task<List<SubtitleModel>> SearchSubtitleAsync(this OsClient client, SearchRequest request, string baseUrl, string userConfigQueryString)
    {
        var httpResponse = await InitiateSearchAsync(client, request);
        var serializedResponse = await SerializeResponseAsync(httpResponse);
        var subtitles = AdaptToStremIo(serializedResponse, baseUrl, userConfigQueryString);

        if (serializedResponse.IsSuccessStatusCode)
            return subtitles;
        
        Log.Error("Failed to get download link: {StatusCode}", serializedResponse.StatusCode);
        throw new UnknownErrorException();
    }

    private static async Task<HttpResponseMessage> InitiateSearchAsync(OsClient client, SearchRequest request)
    {
        var baseUrl = $"{OsConsts.BaseUrl}/subtitles";
        
        // todo: do we need isNullOrEmpty checks here?
        var queryParams = new Dictionary<string, string?>();
    
        queryParams.Add("imdb_id", request.ImdbId);
        
        queryParams.Add("hearing_impaired", request.HearingImpairedOption);

        request.Languages.Sort();
        var languages = string.Join(",", request.Languages);
        queryParams.Add("languages", languages);
    
        if (!string.IsNullOrEmpty(request.SeasonNumber))
            queryParams.Add("season_number", request.SeasonNumber);
        
        if (!string.IsNullOrEmpty(request.EpisodeNumber))
            queryParams.Add("episode_number", request.EpisodeNumber);

        if (queryParams.Count == 0)
        {
            await NotifyViaDiscord.SendNotificationAsync("No query parameters provided for search", DiscordMessageType.Warning);
            throw new InvalidRequestException("No query parameters provided for search");
        }
    
        var url = QueryHelpers.AddQueryString(baseUrl, queryParams);
    
        return await client.HttpClient.GetAsync(url);
    }
    
    // todo: make this generic
    private static async Task<SearchResponse> SerializeResponseAsync(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        var serializedResponse = JsonSerializer.Deserialize<SearchResponse>(responseContent);
        
        serializedResponse.IsSuccessStatusCode = response.IsSuccessStatusCode;
        serializedResponse.StatusCode = (int)response.StatusCode;

        return serializedResponse;
    }
    
    private static List<SubtitleModel> AdaptToStremIo(SearchResponse response, string baseUrl, string userConfigQueryString)
    {
        List<SubtitleModel> subtitles = new();
        
        // Http is not supported by Stremio. If the app is behind a proxy like Traefik, it might think requests are http.
        // todo: There should be a better fix than this.
        if (baseUrl.StartsWith("http://") && (!baseUrl.Contains("localhost") || !baseUrl.Contains("127.0.0.1")))
        {
            baseUrl = baseUrl.Replace("http://", "https://");
            Log.Warning($"Download command URL is http. {Environment.NewLine}" +
                        $"Replaced with https, result: {baseUrl} {Environment.NewLine}");
        }
        
        foreach (var d in response.data)
        {
            var isAITranslated = d.Attributes.AiTranslated ? "(AI)" : "";
            
            var uri = new Uri(new Uri(baseUrl), $"{RouteConsts.SubtitleServeRoute}{userConfigQueryString}&{RouteConsts.FileId}={d.Attributes.Files[0].FileId}");
            var downloadCommandUrl = uri.ToString();
            
            subtitles.Add(new SubtitleModel
            {
                NameToDisplay = $"OpenSubtitles{isAITranslated} - {d.Attributes.Files[0].FileId}",
                Language = LanguageService.FromAlpha2ToAlpha3(d.Attributes.Language),
                DownloadCommand = downloadCommandUrl
            });
        }

        if (subtitles.Count > 0)
            Log.Information("ExampleDownloadCommand: {ExampleDownloadCommand}", subtitles.First().DownloadCommand);
        
        return subtitles;
    }
}

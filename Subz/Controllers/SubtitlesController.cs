using Microsoft.AspNetCore.Http.Extensions;
using Subz.Infrastructure.ConstantsAndEnums;
using Subz.Infrastructure.Services;
using Subz.Models;
using Subz.OpenSubtitleClient.Services;
using Subz.OpenSubtitleClient.Services.Dtos.Search;

namespace Subz.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route(RouteConsts.SubtitleSearchRoute)]
public class SubtitlesController(UserProfileAccessor userProfileAccessor) : ControllerBase
{
    [HttpGet("{**catchAll}")]

    public async Task<IActionResult> GetSubtitlesAsync()
    {
        var imdbId = ExtractImdbId();
        var languagesToSearch = LanguageService.FromAlpha3ToAlpha2(userProfileAccessor.UserProfile.Languages);

        SearchRequest request;
        if (imdbId is { season: "", episode: "" })
        {
            request = new SearchRequest
            {
                ImdbId = imdbId.imdbId,
                HearingImpairedOption = userProfileAccessor.UserProfile.HearingImpairedOption,
                Languages = languagesToSearch
            };
        }
        else
        {
            request = new SearchRequest
            {
                ImdbId = imdbId.imdbId,
                HearingImpairedOption = userProfileAccessor.UserProfile.HearingImpairedOption,
                SeasonNumber = imdbId.season,
                EpisodeNumber = imdbId.episode,
                Languages = languagesToSearch
            };
        }

        var subtitles = await userProfileAccessor.UserProfile.OsClient.SearchSubtitleAsync(request, userProfileAccessor.UserProfile.AppUrl, userProfileAccessor.UserProfile.UserConfigQueryString);
        
        return Ok(new
        {
            subtitles = subtitles
        });
    }

    public (string imdbId, string season, string episode) ExtractImdbId()
    {
        // ExampleUrls:
        // Movie:
        // http://localhost:5014/api/subtitles/movie/tt30988739/filename=Black Bag 2025 576p WEBRip x265 AAC-SSN.mkv&videoSize=1090905980&videoHash=b8e54fe5946daaed.json?AndMyOtherParams
        // Series:
        // http://localhost:5014/api/subtitles/series/tt14269590:1:1/filename=Poker.Face.2023.S01E01.WEBRip.x265-ION265[eztv.re].mp4&videoSize=470245804&videoHash=c6870bb03afe3300.json?AndMyOtherParams
        var fullUrl = Request.GetDisplayUrl();

        var mediaInfo = fullUrl
            .Replace(userProfileAccessor.UserProfile.AppUrl, "")
            .Replace(RouteConsts.SubtitleSearchRoute, "")
            .Trim('/')
            .Split("/filename=")
            .First();

        if (mediaInfo.StartsWith("movie"))
        {
            var imdId = mediaInfo.Replace("movie/", "");
            return (
                imdbId: imdId, 
                season: string.Empty,
                episode: string.Empty);
        }
        else
        {
            var allParts = mediaInfo.Replace("series/", "").Split(':');
            return (
                imdbId: allParts[0], 
                season: allParts[1],
                episode: allParts[2]);
        }
    }
}
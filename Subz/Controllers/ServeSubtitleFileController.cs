using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Subz.Infrastructure.ConstantsAndEnums;
using Subz.Infrastructure.Exceptions;
using Subz.Models;
using Subz.OpenSubtitleClient.Services;
using Subz.OpenSubtitleClient.Services.Dtos.Download;

namespace Subz.Controllers;

[ApiController]
[Route($"{RouteConsts.SubtitleServeRoute}")]
public class ServeSubtitleFileController : ControllerBase
{
    private readonly UserProfileAccessor _userProfileAccessor;
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TimeSpan _cacheExpirationTime = TimeSpan.FromHours(4);

    public ServeSubtitleFileController(
        UserProfileAccessor userProfileAccessor,
        IMemoryCache memoryCache,
        IHttpClientFactory httpClientFactory)
    {
        _userProfileAccessor = userProfileAccessor;
        _memoryCache = memoryCache;
        _httpClientFactory = httpClientFactory;
    }

    // todo: Cant I just redirect to the download url?
    [HttpGet]
    public async Task<IActionResult> ServeSubtitleFile()
    {
        try
        {
            var userProfile = _userProfileAccessor.UserProfile;

            var fileId = ExtractFileId();
            
            string cacheKey = $"subtitle_file_{fileId}";
            
            if (_memoryCache.TryGetValue(cacheKey, out CachedSubtitleFile cachedFile))
            {
                return File(cachedFile.Content, cachedFile.ContentType, cachedFile.FileName);
            }
            
            var downloadResponse = await userProfile.OsClient.DownloadSubtitleAsync(new DownloadRequest { FileId = fileId });
            var downloadUrl = downloadResponse.Link;
            
            if (string.IsNullOrEmpty(downloadUrl))
            {
                return NotFound("Download URL not available");
            }
            
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(downloadUrl);
            
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to download subtitle file");
            }
            
            // Get the content
            var content = await response.Content.ReadAsByteArrayAsync();
            
            // Detect content type based on file extension
            var fileName = downloadResponse.FileName ?? $"subtitle_{fileId}.vtt";
            var contentType = DetermineContentType(fileName);
            
            // Cache the file
            var subtitleFile = new CachedSubtitleFile
            {
                Content = content,
                ContentType = contentType,
                FileName = fileName
            };
            
            // Add to cache with expiration
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(_cacheExpirationTime)
                .SetPriority(CacheItemPriority.Normal);
                
            _memoryCache.Set(cacheKey, subtitleFile, cacheOptions);
            
            // Return the file
            return File(content, contentType, fileName);
        }
        catch (Exception ex)
        {
            // Log exception
            return StatusCode(500, $"Error serving subtitle file: {ex.Message}");
        }
    }
    
    private string ExtractFileId()
    {
        if (HttpContext.Request.Query.TryGetValue(RouteConsts.FileId, out var fileIdValues) && 
            !string.IsNullOrEmpty(fileIdValues.FirstOrDefault()))
        {
            return fileIdValues.First();
        }

        throw new InvalidRequestException("File ID not found in the query parameters");
    }

    private string DetermineContentType(string fileName)
    {
        return Path.GetExtension(fileName).ToLower() switch
        {
            ".vtt" => "text/vtt",
            ".srt" => "application/x-subrip",
            ".ass" => "text/plain",
            ".ssa" => "text/plain",
            _ => "application/octet-stream"
        };
    }
    
    private class CachedSubtitleFile
    {
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}

using Subz.Consts;
using Subz.Infrastructure.ConstantsAndEnums;
using Subz.Infrastructure.Exceptions;
using Subz.Infrastructure.Services;
using Subz.Models;
using Subz.OpenSubtitleClient;
using Subz.Services;

namespace Subz.Middlewares;

public class ConfigCacheMiddleware(RequestDelegate next, CacheService cacheService, UserProfileAccessor userProfileAccessor)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var dto = ExtractUserProfileDtoFromQueryString(context);
            
            var userProfile = cacheService.TryToGetUserProfileFromCache(dto) ?? await CreateAndCacheUserProfileAsync(dto);

            userProfileAccessor.UserProfile = userProfile;
        }
        catch (Exception e)
        {
            // TODO: THIS WILL NOT WORK
            await NotifyViaDiscord.SendNotificationAsync($"Error in ConfigCacheMiddleware: {e.Message}", DiscordMessageType.Error);
            throw;
        }
        
        await next(context);
    }

    private UserProfileDto ExtractUserProfileDtoFromQueryString(HttpContext context)
    {
        var queryParams = context.Request.Query;

        queryParams.TryGetValue(OsQueryParamConsts.Username, out var username);
        queryParams.TryGetValue(OsQueryParamConsts.Password, out var password);
        queryParams.TryGetValue(OsQueryParamConsts.ApiKey, out var apiKey);
        queryParams.TryGetValue(ConfigurationQueryParamConsts.SelectedLanguages, out var selectedLanguagesParam);
        queryParams.TryGetValue(ConfigurationQueryParamConsts.HearingImpairedOption, out var hearingImpairedOption);
        queryParams.TryGetValue(ConfigurationQueryParamConsts.DiscordWebhook, out var discordWebhook);
        
        var selectedLanguages = selectedLanguagesParam.ToString().Split(',').Select(lang => lang.Trim()).ToList();

        if (string.IsNullOrEmpty(username) ||
            string.IsNullOrEmpty(password) || 
            string.IsNullOrEmpty(apiKey) ||
            selectedLanguages.Count == 0 ||
            string.IsNullOrEmpty(hearingImpairedOption)
            )
        {
            Log.Error("Missing one or more required query parameters");
            throw new InvalidConfigException();
        }
        
        return new UserProfileDto
        {
            OsUsername = username.ToString(),
            OsPassword = password.ToString(),
            OsApiKey = apiKey.ToString(),
            
            SelectedLanguages = selectedLanguages,
            HearingImpairedOption = hearingImpairedOption.ToString(),
            
            DiscordWebhook = discordWebhook.ToString(),
            WindowUrl = $"{context.Request.Scheme}://{context.Request.Host}"
        };
    }
    
    private async Task<UserProfile> CreateAndCacheUserProfileAsync(UserProfileDto dto)
    {
        var userProfile = await UserProfileFactory.CreateAsync(dto);
        
        cacheService.AddUserProfileToCache(userProfile);

        return userProfile;
    }
}

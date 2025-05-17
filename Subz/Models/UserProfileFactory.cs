using Subz.Components;
using Subz.Consts;
using Subz.Infrastructure.ConstantsAndEnums;
using Subz.Infrastructure.Exceptions;
using Subz.Infrastructure.Services;
using Subz.OpenSubtitleClient;

namespace Subz.Models;

public static class UserProfileFactory
{
    public static async Task<UserProfile> CreateAsync(UserProfileDto dto)
    {
        ValidateValues(dto);
        
        var client = await CreateOsClientAsync(dto);

        var queryString = GenerateConfigQueryString(dto);

        return new UserProfile
        {
            OsClient = client,
            Languages = dto.SelectedLanguages,
            HearingImpairedOption = dto.HearingImpairedOption,
            DiscordWebhook = dto.DiscordWebhook,
            InstallAddonUrl = $"{dto.WindowUrl}/{RouteConsts.Manifest}{queryString}",
            AppUrl = dto.WindowUrl,
            UserConfigQueryString = queryString
        };
    }
    
    public static async Task<UserProfile> CreateAsync(
        OpenSubtitleCredentialsBlock openSubtitleCredentialsBlock,
        SubtitleLanguageSelectionBlock languageBlock,
        HearingImpairmentBlock hearingImpairmentBlock,
        DiscordNotificationHookBlock discordNotificationHookBlock,
        string windowUrl
    )
    {
        var username = openSubtitleCredentialsBlock.Username;
        var password = openSubtitleCredentialsBlock.Password;
        var apiKey = openSubtitleCredentialsBlock.ApiKey;
        
        var languages = await languageBlock.GetLanguageDataAsync();
        var hearingImpairedOption = hearingImpairmentBlock.HearingImpairedOption.ToString();
        var discordWebhook = discordNotificationHookBlock.DiscordWebhook;

        return await CreateAsync(new UserProfileDto
        {
            OsUsername = username,
            OsPassword = password,
            OsApiKey = apiKey,
            SelectedLanguages = languages,
            HearingImpairedOption = hearingImpairedOption,
            DiscordWebhook = discordWebhook,
            WindowUrl = windowUrl
        });
    }

    // TODO: REMOVE
    // public static async Task<UserProfile> CreateFromUiComponentsAsync(
    //     OpenSubtitleCredentialsBlock openSubtitleCredentialsBlock,
    //     SubtitleLanguageSelectionBlock languageBlock,
    //     HearingImpairmentBlock hearingImpairmentBlock,
    //     DiscordNotificationHookBlock discordNotificationHookBlock,
    //     string windowUrl
    //     )
    // {
    //     var username = openSubtitleCredentialsBlock.Username;
    //     var password = openSubtitleCredentialsBlock.Password;
    //     var apiKey = openSubtitleCredentialsBlock.ApiKey;
    //     
    //     var languages = await languageBlock.GetLanguageDataAsync();
    //     var hearingImpairedOption = hearingImpairmentBlock.HearingImpairedOption.ToString();
    //     var discordWebhook = discordNotificationHookBlock.DiscordWebhook;
    //     
    //     await ValidateValuesAsync(username, password, apiKey, languages, hearingImpairedOption, discordWebhook);
    //     
    //     var client = await CreateOsClientAsync(username, password, apiKey);
    //
    //     var installUrl = GenerateInstallUrl(username, password, apiKey, hearingImpairedOption, languages, discordWebhook, windowUrl);
    //
    //     return new UserProfile
    //     {
    //         OsClient = client,
    //         Languages = languages,
    //         HearingImpairedOption = hearingImpairedOption,
    //         DiscordWebhook = discordWebhook,
    //         InstallUrl = installUrl
    //     };
    // }
    
    private static void ValidateValues(UserProfileDto dto)
    {
        var errMsg = CheckIfEmpty(dto);

        if (errMsg.Length > 0)
            throw new InvalidConfigException(errMsg);
        
        if (!string.IsNullOrEmpty(dto.DiscordWebhook) && !Uri.IsWellFormedUriString(dto.DiscordWebhook, UriKind.Absolute))
            throw new InvalidConfigException("Discord webhook value is not a valid URL.");
    }
    
    private static async Task<OsClient> CreateOsClientAsync(UserProfileDto dto)
    {
        try
        {
            return await OsClientFactory.CreateAsync(dto.OsUsername, dto.OsPassword, dto.OsApiKey);
        }
        catch (Exception e)
        {
            throw new InvalidConfigException($"Failed to login to OpenSubtitles. {Environment.NewLine}Credentials are probably wrong. {Environment.NewLine}Error message: {e.Message}");
        }
    }
    
    private static string CheckIfEmpty(UserProfileDto dto)
    {
        var errMsg = "";
        
        if (string.IsNullOrEmpty(dto.OsUsername))
            errMsg += "Username ";
        
        if (string.IsNullOrEmpty(dto.OsPassword))
            errMsg += "Password ";
        
        if (string.IsNullOrEmpty(dto.OsApiKey))
            errMsg += "API Key ";
        
        if (string.IsNullOrEmpty(dto.HearingImpairedOption))
            errMsg += "Hearing impairment option ";

        if (errMsg.Length > 0)
        {
            errMsg += "cannot be empty. ";
        }
        
        if (dto.SelectedLanguages.Count == 0)
            errMsg += "No languages selected. ";

        return errMsg;
    }

    private static string GenerateConfigQueryString(UserProfileDto dto)
    {
        var languagesAlpha3Codes = LanguageService.FromNameToAlpha3(dto.SelectedLanguages);
        
        var queryParams = new List<string>
        {
            $"{OsQueryParamConsts.Username}={Uri.EscapeDataString(dto.OsUsername)}",
            $"{OsQueryParamConsts.Password}={Uri.EscapeDataString(dto.OsPassword)}",
            $"{OsQueryParamConsts.ApiKey}={Uri.EscapeDataString(dto.OsApiKey)}",
            $"{ConfigurationQueryParamConsts.HearingImpairedOption}={Uri.EscapeDataString(dto.HearingImpairedOption)}",
            $"{ConfigurationQueryParamConsts.SelectedLanguages}={Uri.EscapeDataString(string.Join(",", languagesAlpha3Codes))}"
        };

        if (!string.IsNullOrEmpty(dto.DiscordWebhook))
            queryParams.Add($"{ConfigurationQueryParamConsts.DiscordWebhook}={Uri.EscapeDataString(dto.DiscordWebhook)}");
        
        return "?" + string.Join("&", queryParams);
    }
    // ?OsUsername=husoyo&OsPassword=U9zsB%23e%21GzK%40&OsApiKey=NQjBPZI4rILU3gYgZX83CCbKLu75XXRi&hi=Include&langs=eng%2Ctur&dw=https%3A%2F%2Fdiscord.com%2Fapi%2Fwebhooks%2F1369682476930433164%2FE-l_J_XmdMwLS9J-HsAnOMevBJGnYNZL3eWYGOIx26tm3NGT5qnU2FlgflT7eVmFA_Wn
}

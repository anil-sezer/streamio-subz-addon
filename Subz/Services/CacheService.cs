using Microsoft.Extensions.Caching.Memory;
using Subz.Models;

namespace Subz.Services;

// todo: Do we use this effectively?
public class CacheService(IMemoryCache memoryCache)
{
    public UserProfile? TryToGetUserProfileFromCache(string username, string password)
    {
        return memoryCache.TryGetValue(GetUserProfileCacheKey(username, password), out UserProfile userProfile) ? userProfile : null;
    }
    
    public UserProfile? TryToGetUserProfileFromCache(UserProfileDto dto)
    {
        return TryToGetUserProfileFromCache(dto.OsUsername, dto.OsPassword);
    }
    
    public void AddUserProfileToCache(UserProfile u)
    {
        memoryCache.Set(GetUserProfileCacheKey(u.OsClient.Username, u.OsClient.Password), u, TimeSpan.FromMinutes(30));
    }
    
    private static string GetUserProfileCacheKey(string username, string password)
    {
        return $"{username}{password}";
    }
}
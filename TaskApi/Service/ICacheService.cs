using TaskApi.Models;
namespace TaskApi.Helpers;

public interface ICacheService
{
    Task<CacheUserDTO?> GetUserFromCache(int user_id);
    Task SetUserToCache(CacheUserDTO user, TimeSpan? expiry = null);
    
}


using System.Text.Json;
 
using StackExchange.Redis;
using TaskApi.Helpers;
using TaskApi.Models;
using TaskApi.Services;

public class CacheService : ICacheService
{

    private readonly IDatabase _db;
    private readonly IUserService _service;

    public CacheService(IConnectionMultiplexer redis, IUserService service)
    {
        _db = redis.GetDatabase();
        _service = service;
    }

    public async Task<CacheUserDTO?> GetUserFromCache(int user_id)
    {
        var data = await _db.StringGetAsync($"user:{user_id}:profile");
        if (data.IsNullOrEmpty)
        {
            throw new Exception("Returned null");
        }

        return JsonSerializer.Deserialize<CacheUserDTO>(data!);
    }

    public async Task SetUserToCache(CacheUserDTO user, TimeSpan? expiry = null)
    {
        var data = JsonSerializer.Serialize(user);
        await _db.StringSetAsync($"user: {user.user_id}:profile", data, expiry ?? TimeSpan.FromMinutes(10));
        
    }
}
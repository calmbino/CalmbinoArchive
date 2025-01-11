using System.Text.Json;
using CalmbinoArchive.Application.Interfaces;
using StackExchange.Redis;

namespace CalmbinoArchive.Infrastructure.Services;

public class CacheService(IConnectionMultiplexer connection) : ICacheService
{
    private readonly IDatabase _cache = connection.GetDatabase();

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        var cachedValue = await _cache.StringGetAsync(key);

        if (cachedValue.IsNull)
        {
            return null;
        }

        return JsonSerializer.Deserialize<T>(cachedValue!);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null) where T : class
    {
        var stringValue = JsonSerializer.Serialize(value);

        await _cache.StringSetAsync(key, stringValue, expiry);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.KeyDeleteAsync(key);
    }

    public async Task RemoveByPrefixAsync(string prefix)
    {
        var endPoints = connection.GetEndPoints();
        var removeTasks = endPoints.Select(async endPoint =>
        {
            var server = connection.GetServer(endPoint);

            var keys = server.Keys(pattern: $"{prefix}*")
                             .ToArray();

            if (keys.Length > 0)
            {
                await _cache.KeyDeleteAsync(keys);
            }
        });

        await Task.WhenAll(removeTasks);
    }
}
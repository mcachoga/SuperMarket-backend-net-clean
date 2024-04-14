using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace SuperMarket.Infrastructure.Extensions.Caching;

public static class IDistributedCacheExtensions
{
    public static async Task<T> GetKeyAsync<T>(this IDistributedCache distributedCache, 
        string cacheKey, CancellationToken token = default)
    {
        if (string.IsNullOrEmpty(cacheKey)) return default;

        var dataInBytes = await distributedCache.GetAsync(cacheKey, token).ConfigureAwait(false);

        if (dataInBytes != null)
        {
            var rawJson = Encoding.UTF8.GetString(dataInBytes);
            return JsonSerializer.Deserialize<T>(rawJson);
        }

        return default;
    }

    public static async Task RemoveKeyAsync(this IDistributedCache distributedCache, 
        string cacheKey, CancellationToken token = default)
    {
        if (string.IsNullOrEmpty(cacheKey)) return;

        await distributedCache.RemoveAsync(cacheKey, token).ConfigureAwait(false);
    }

    public static async Task SetKeyAsync<T>(this IDistributedCache distributedCache, 
        string cacheKey, T obj, int cacheExpirationInMinutes = 30, CancellationToken token = default)
    {
        if (string.IsNullOrEmpty(cacheKey) || obj is null) return;

        DistributedCacheEntryOptions options = 
            new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(cacheExpirationInMinutes));

        var dataInBytes = JsonSerializer.SerializeToUtf8Bytes<T>(obj);

        await distributedCache.SetAsync(cacheKey, dataInBytes, options, token).ConfigureAwait(false);
    }
}
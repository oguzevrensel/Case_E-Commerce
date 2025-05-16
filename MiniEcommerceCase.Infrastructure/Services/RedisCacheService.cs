using StackExchange.Redis;
using System.Text.Json;
using MiniEcommerceCase.Application.Interfaces;

namespace MiniEcommerceCase.Infrastructure.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly StackExchange.Redis.IDatabase _cache;

        public RedisCacheService()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _cache = redis.GetDatabase();
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? ttl = null)
        {
            var json = JsonSerializer.Serialize(value);
            await _cache.StringSetAsync(key, json, ttl);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _cache.StringGetAsync(key);
            return value.HasValue
                ? JsonSerializer.Deserialize<T>(value!)
                : default;
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.KeyDeleteAsync(key);
        }
    }
}

using Resume.Application.Redis.Caching.Interfaces;
using Resume.Infra.Data.Context;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Resume.Infra.Data.Services.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;
        private readonly IConnectionMultiplexer _connection;

        public RedisCacheService(IConnectionMultiplexer connection)
        {
            _connection = connection;
            _db = connection.GetDatabase();
        }
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var jsonData = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, jsonData, expiry);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);

            if (value.IsNullOrEmpty) return default;

            return JsonSerializer.Deserialize<T>(value!);


        }
        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        public async Task RemoveByPatternAsync(string pattern)
        {
            var server = _connection.GetServer(_connection.GetEndPoints().First());
            var keys = server.Keys(pattern: $"*{pattern}*").ToArray();

            foreach (var key in keys)
            {
                await _db.KeyDeleteAsync(key);
            }
        }
    }
}

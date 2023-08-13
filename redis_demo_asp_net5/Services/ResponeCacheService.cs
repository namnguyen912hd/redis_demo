using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace redis_demo_asp_net5.Services
{
    public class ResponeCacheService : IResponeCacheService
    {
        private readonly IDistributedCache  _distributedCache;

        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public ResponeCacheService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
        {
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<string> GetCacheResponeAsync(string cacheKey)
        {
            if (string.IsNullOrWhiteSpace(cacheKey))
                return null;

            var cacheRespone = await _distributedCache.GetStringAsync(cacheKey);
            return cacheRespone;

        }

        public async Task SetCacheResponeAsync(string cacheKey, object respone, TimeSpan timeOut)
        {
            if (respone == null)
                return;

            var serializerRespone = JsonConvert.SerializeObject(respone, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            await _distributedCache.SetStringAsync(cacheKey, serializerRespone, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = timeOut
            });

        }


        public async Task RemoveCacheResponeAsync(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                throw new ArgumentException("value can not be null");
            }

            await foreach (var key in GetKeyAsync(pattern + "*"))
            {
                await _distributedCache.RemoveAsync(key);
            }

        }


        private async IAsyncEnumerable<string> GetKeyAsync(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                throw new ArgumentException("value can not be null");
            }

            foreach (var endPoint in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(endPoint);
                foreach (var key in server.Keys(pattern:pattern))
                {
                    yield return key.ToString();
                }
            }

        }

    }
}

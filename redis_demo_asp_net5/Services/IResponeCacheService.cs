using System;
using System.Threading.Tasks;

namespace redis_demo_asp_net5.Services
{
    public interface IResponeCacheService
    {
        Task SetCacheResponeAsync(string cacheKey, object respone, TimeSpan timeOut);

        Task<string> GetCacheResponeAsync(string cacheKey);

        Task RemoveCacheResponeAsync(string pattern);

    }
}

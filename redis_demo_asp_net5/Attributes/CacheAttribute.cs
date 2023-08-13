using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using redis_demo_asp_net5.Configurations;
using redis_demo_asp_net5.Services;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace redis_demo_asp_net5.Attributes
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;

        public CacheAttribute(int timeToLiveSeconds = 1000)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheConfiguration = context.HttpContext.RequestServices.GetRequiredService<RedisConfiguration>();
            //check cache exists
            if (!cacheConfiguration.Enabled)
            {
                await next();
                return;
            }

            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponeCacheService>();

            var cacheKey = GenerateCacheKeyFromResquest(context.HttpContext.Request);

            var cacheRespone = await cacheService.GetCacheResponeAsync(cacheKey);

            if (!string.IsNullOrWhiteSpace(cacheRespone))
            {
                var contentResult = new ContentResult() { 
                    Content = cacheRespone,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

            var excutedContext = await next();
            if (excutedContext.Result is OkObjectResult okObjectResult)
            {
                await cacheService.SetCacheResponeAsync(cacheKey, okObjectResult, TimeSpan.FromSeconds(_timeToLiveSeconds));
            }

        }

        private static string GenerateCacheKeyFromResquest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}--{value}");
            }

            return keyBuilder.ToString();
        }
    }
}

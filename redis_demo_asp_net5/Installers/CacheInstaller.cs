using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using redis_demo_asp_net5.Configurations;
using redis_demo_asp_net5.Services;
using StackExchange.Redis;

namespace redis_demo_asp_net5.Installers
{
    public class CacheInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var redisConfiguration = new RedisConfiguration();
            configuration.GetSection("RedisConfiguration").Bind(redisConfiguration);
            services.AddSingleton(redisConfiguration);

            if (!redisConfiguration.Enabled)
                return;

            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConfiguration.ConnectionString));
            services.AddStackExchangeRedisCache(option => option.Configuration = redisConfiguration.ConnectionString);
            services.AddSingleton<IResponeCacheService, ResponeCacheService>();

        }
    }
}

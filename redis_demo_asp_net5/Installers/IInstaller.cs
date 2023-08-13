using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace redis_demo_asp_net5.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}

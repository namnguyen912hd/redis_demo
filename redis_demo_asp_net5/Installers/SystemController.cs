using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace redis_demo_asp_net5.Installers
{
    public class SystemController : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            //services.AddMvc();
        }
    }
}

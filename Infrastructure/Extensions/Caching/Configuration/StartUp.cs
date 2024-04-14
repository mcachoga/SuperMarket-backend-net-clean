using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SuperMarket.Infrastructure.Extensions.Caching
{
    public static class StartUp
    {
        public static void AddCustomConfigurationCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedMemoryCache();
        }
    }
}
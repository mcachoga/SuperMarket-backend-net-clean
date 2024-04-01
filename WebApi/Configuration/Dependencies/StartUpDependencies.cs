using SuperMarket.Application.Configuration;
using SuperMarket.Infrastructure.Configuration;
using SuperMarket.Infrastructure.Context;

namespace SuperMarket.WebApi.Configuration
{
    public static class StartUpDependencies
    {
        public static void AddCustomConfigurationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationServices();

            services.AddDatabase(configuration);
            services.AddIdentityServices();
            services.AddCatalogServices();
            services.AddInfrastructureDependencies();
        }

        public static void UseCustomDependencies(this IApplicationBuilder app)
        {
            app.SeedDatabase();
        }

        private static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var seeders = serviceScope.ServiceProvider.GetServices<ApplicationDbSeeeder>();

            foreach (var seeder in seeders)
            {
                seeder.SeedDatabaseAsync().GetAwaiter().GetResult();
            }

            return app;
        }
    }
}
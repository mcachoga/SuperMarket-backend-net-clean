using MediatR;
using SuperMarket.Application.Configuration;
using SuperMarket.Application.Identity.Configuration;
using SuperMarket.Infrastructure.Framework.Validations;
using SuperMarket.Persistence.Configuration;
using SuperMarket.Persistence.Context;
using SuperMarket.Persistence.Identity.Configuration;
using SuperMarket.Persistence.Identity.Context;

namespace SuperMarket.WebApi.Configuration
{
    public static class StartUpDependencies
    {
        public static void AddCustomConfigurationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationDependencies();
            services.AddApplicationIdentityDependencies();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));

            services.AddDatabase(configuration);
            services.AddDatabaseIdentity(configuration);

            services.AddIdentityServices();
            services.AddCatalogServices(configuration);
        }

        public static void UseCustomDependencies(this IApplicationBuilder app, IConfiguration configuration)
        {
            var executeSeedingData = configuration.GetValue<bool>("ExecuteSeedingData");

            app.InitializeDatabase(executeSeedingData);
            app.InitializeDatabaseIdentity(executeSeedingData);

        }
        private static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app, bool applySeedData)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var seeders = serviceScope.ServiceProvider.GetServices<ApplicationDbSeeeder>();

            foreach (var seeder in seeders)
            {
                seeder.ConfigureDatabaseAsync(applySeedData).GetAwaiter().GetResult();
            }

            return app;
        }

        private static IApplicationBuilder InitializeDatabaseIdentity(this IApplicationBuilder app, bool applySeedData)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var seeders = serviceScope.ServiceProvider.GetServices<ApplicationIdentityDbSeeder>();

            foreach (var seeder in seeders)
            {
                seeder.ConfigureDatabaseAsync(applySeedData).GetAwaiter().GetResult();
            }

            return app;
        }
    }
}
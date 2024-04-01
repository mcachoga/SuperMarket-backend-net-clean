using SuperMarket.WebApi.Configuration.Middlewares;

namespace SuperMarket.WebApi.Configuration
{
    public static class StartUpApi
    {
        public static void AddCustomConfigurationApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
        }

        public static void UseCustomApi(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
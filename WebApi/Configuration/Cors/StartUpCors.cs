using SuperMarket.Infrastructure.Extensions.Configurations;

namespace SuperMarket.WebApi.Configuration
{
    public static class StartUpCors
    {
        public static void AddCustomConfigurationCors(this IServiceCollection services, IConfiguration configuration)
        {
            var corsSettings = configuration.GetSectionToModel<CorsSettings>();

            services.AddCors(o =>
                o.AddPolicy(corsSettings.PolicyName, builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                }
            ));
        }

        public static void UseCustomCors(this IApplicationBuilder app, IConfiguration configuration)
        {
            var corsSettings = configuration.GetSectionToModel<CorsSettings>();

            app.UseCors(corsSettings.PolicyName);
        }
    }
}
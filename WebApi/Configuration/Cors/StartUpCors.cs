using SuperMarket.Infrastructure.Framework.Configurations;

namespace SuperMarket.WebApi.Configuration
{
    public static class StartUpCors
    {
        public static void AddCustomConfigurationCors(this IServiceCollection services, IConfiguration configuration)
        {
            var corsSettings = configuration.GetSectionToModel<CorsSettings>();

            if (corsSettings == null) return;

            services.AddCors(options =>
                options.AddPolicy(corsSettings.PolicyName, builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                       // .AllowCredentials()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithOrigins(corsSettings.AllowedOrigins != null ? corsSettings.AllowedOrigins : new string[] { });
                        
                }
            ));
        }

        public static void UseCustomCors(this IApplicationBuilder app, IConfiguration configuration)
        {
            var corsSettings = configuration.GetSectionToModel<CorsSettings>();

            if (corsSettings != null)
            {
                app.UseCors(corsSettings.PolicyName);
            }
        }
    }
}
using Microsoft.OpenApi.Models;
using SuperMarket.Infrastructure.Framework.Configurations;

namespace SuperMarket.WebApi.Configuration
{
    public static class StartUpSwagger
    {
        public static void AddCustomConfigurationSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var swaggerSettings = configuration.GetSectionToModel<SwaggerSettings>();

            services.AddSwaggerGen(options =>
            {               
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });

                options.SwaggerDoc(swaggerSettings.ApiVersion, new OpenApiInfo
                {
                    Version = swaggerSettings.ApiVersion,
                    Title = swaggerSettings.ApiName,
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
            });
        }

        public static void UseCustomSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            var swaggerSettings = configuration.GetSectionToModel<SwaggerSettings>();

            if (swaggerSettings.UseSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }
    }
}
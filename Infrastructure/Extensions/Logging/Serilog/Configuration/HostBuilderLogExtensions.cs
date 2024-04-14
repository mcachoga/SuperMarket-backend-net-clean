using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SuperMarket.Infrastructure.Extensions.Logging
{
    public static class HostBuilderLogExtensions
    {
        public static IHostBuilder UseSerilog(this IHostBuilder builder, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            SerilogHostBuilderExtensions.UseSerilog(builder);

            return builder;
        }

        /// <summary>
        /// Permite usar Serilog en los logging de request logging en el proceso de request-pipeline.
        /// Se debería de llamar lo antes posible en la pipeline para capturar la mayor cantidad de logs posible
        /// </summary>
        public static void UseCustomSerilog(this IApplicationBuilder app)
        {
            app
                .UseSerilogRequestLogging(
                    opts => opts.EnrichDiagnosticContext = LogEnricherExtensions.EnrichFromRequest
                    // Eliminar logs de heath endpoints
                    //opts => opts.GetLevel = LogHelper.ExcludeHealthChecks
                );
        }
    }
}
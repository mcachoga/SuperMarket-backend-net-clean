using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SuperMarket.Infrastructure.Extensions.Logging
{
    public static class HostBuilderLogExtensions
    {
        public static IHostBuilder UseSerilog(this IHostBuilder builder)
        {
            var config = new ConfigurationBuilder().AddJsonFile("AppSettings.json").AddEnvironmentVariables().Build();

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();

            SerilogHostBuilderExtensions.UseSerilog(builder);

            return builder;

            ////var serilogCustomConfig = configuration.GetSectionToModel<SerilogSettings>();

            ////var serilogConfig = new LoggerConfiguration().ReadFrom.Configuration(configuration);

            ////builder = builder.UseSerilog( (context, serilogConfig) =>
            ////{
            ////    var providerLog = context.HostingEnvironment.IsDevelopment() ?
            ////        serilogCustomConfig.WriteDefaultDev.ToLower() : 
            ////        serilogCustomConfig.WriteDefaultPro.ToLower();

            ////    if (providerLog.Equals("rollingfile"))
            ////    {
            ////        serilogConfig.WriteTo.File(path: serilogCustomConfig.FilePathFormat, outputTemplate: serilogCustomConfig.FileTemplate);
            ////    }
            ////    else if (providerLog.Equals("mssqlserver"))
            ////    {

            ////    }
            ////    else
            ////    {
            ////        serilogConfig.WriteTo.Console(outputTemplate: serilogCustomConfig.ConsoleTemplate);
            ////    }

            ////});

            ////return builder;
        }

        /// <summary>
        /// Permite usar Serilog en los logging de request logging en el proceso de request-pipeline.
        /// Se debería de llamar lo antes posible en la pipeline para capturar la mayor cantidad de logs posible
        /// </summary>
        ////public static IApplicationBuilder UseMyRequestLogging(this IApplicationBuilder appBuilder)
        ////{
        ////    return appBuilder
        ////        // Log requests
        ////        .UseSerilogRequestLogging(
        ////            // Don't log health check endpoints
        ////            opts => opts.GetLevel = LogHelper.ExcludeHealthChecks);
        ////}
    }
}
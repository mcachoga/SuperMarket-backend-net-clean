using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SuperMarket.Infrastructure.Logging
{
    public static class HostBuilderLogExtensions
    {
        public static IHostBuilder UseSerilog(this IHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json")
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            SerilogHostBuilderExtensions.UseSerilog(builder);

            return builder;
        }
    }
}
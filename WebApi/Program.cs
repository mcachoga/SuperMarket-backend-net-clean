using SuperMarket.Infrastructure.Extensions.Logging;
using SuperMarket.WebApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseSerilog(builder.Configuration)
    .ConfigureAppConfiguration((context, config) =>
    {
        // En esta configuración se debería de asignar la información de Azure.
        // Tambien se podrían asignar los secretos de usuario (actualmente todo se define en appsettings),
        // pero no debería de estar ahí por seguridad:
        //config.AddUserSecrets(Assembly.GetEntryAssembly(), optional: true);
        //config.AddCustomConfigurationAzure(context);
    });


var startup = new Startup(builder.Configuration, builder.Environment);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
app.UseCustomSerilog();

app.Logger.LogInformation("SuperMarket API Configuration Starting...");
startup.Configure(app);
app.Logger.LogInformation("SuperMarket API Complete.");
app.Run();
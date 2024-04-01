using SuperMarket.Infrastructure.Extensions.Logging;
using SuperMarket.WebApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseSerilog()
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
startup.Configure(app);
app.Run();
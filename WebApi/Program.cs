using SuperMarket.Infrastructure.Extensions.Logging;
using SuperMarket.WebApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseSerilog(builder.Configuration)
    .ConfigureAppConfiguration((context, config) =>
    {
        // En esta configuraci�n se deber�a de asignar la informaci�n de Azure.
        // Tambien se podr�an asignar los secretos de usuario (actualmente todo se define en appsettings),
        // pero no deber�a de estar ah� por seguridad:
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
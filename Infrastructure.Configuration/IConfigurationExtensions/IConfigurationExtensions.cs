﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace SuperMarket.Infrastructure.Extensions.Configurations;

public static class ConfigurationExtensions
{
    const string ConfigMissingErrorMessage = "Options type of '{0}' was requested as required, but the corresponding section was not found in configuration. Make sure one of your configuration sources contains this section.";

    public static T GetSectionToModel<T>(this IConfiguration configuration, bool requiredToExistInConfiguration = false) 
        where T : class, new()
    {
        var bound = configuration.GetSection(typeof(T).Name).Get<T>();

        if (bound is null && requiredToExistInConfiguration)
            throw new InvalidOperationException(string.Format(ConfigMissingErrorMessage, typeof(T).Name));

        bound ??= new T();
        Validator.ValidateObject(bound, new ValidationContext(bound), validateAllProperties: true);

        return bound;
    }

    public static void SetModelToSection<T>(this IServiceCollection services, bool requiredToExistInConfiguration = true) 
        where T : class
    {
        var optionsBuilder = services.AddOptions<T>()
            .BindConfiguration(typeof(T).Name)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        if (requiredToExistInConfiguration)
            optionsBuilder.Validate<IConfiguration>((_, configuration)
                => configuration.GetSection(typeof(T).Name).Exists(), string.Format(ConfigMissingErrorMessage, typeof(T).Name));

        services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<T>>().Value);
    }
}
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SuperMarket.Application.Configuration
{
    public static class StartUp
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            return services
                .AddMediatR(assembly)
                .AddAutoMapper(assembly)
                .AddValidatorsFromAssembly(assembly);
        }
    }
}
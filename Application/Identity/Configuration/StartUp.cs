using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SuperMarket.Infrastructure.Framework.Validations;
using System.Reflection;

namespace SuperMarket.Application.Identity.Configuration
{
    public static class StartUp
    {
        public static IServiceCollection AddApplicationIdentityDependencies(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            return services
                .AddMediatR(assembly)
                .AddAutoMapper(assembly)
                .AddValidatorsFromAssembly(assembly);
                
        }
    }
}
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SuperMarket.Application.Validations.Pipelines;
using System.Reflection;

namespace SuperMarket.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            return services
                .AddMediatR(assembly)
                .AddAutoMapper(assembly)
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));
        }
    }
}
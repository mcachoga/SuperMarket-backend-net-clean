using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuperMarket.Application.Services;
using SuperMarket.Application.Services.Identity;
using SuperMarket.Infrastructure.Context;
using SuperMarket.Infrastructure.Services;
using SuperMarket.Infrastructure.Services.Identity;
using System.Reflection;

namespace SuperMarket.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                .AddTransient<ApplicationDbSeeeder>()
                .AddHttpContextAccessor().AddScoped<ICurrentUserService, CurrentUserService>();
            
            return services;
        }

        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services
                .AddTransient<ITokenService, TokenService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IRoleService, RoleService>()
                .AddHttpContextAccessor().AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }

        public static IServiceCollection AddEmployeeService(this IServiceCollection services)
        {
            services.AddTransient<IMarketService, MarketService>();
            services.AddTransient<IProductService, ProductService>();

            services.AddTransient<IOrderService, OrderService>()
                .AddHttpContextAccessor().AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }

        public static void AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

    }
}
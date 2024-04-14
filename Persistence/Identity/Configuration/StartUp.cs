using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuperMarket.Application.Identity.Services.Contracts;
using SuperMarket.Persistence.Identity.Context;
using SuperMarket.Persistence.Identity.Services;

namespace SuperMarket.Persistence.Identity.Configuration
{
    public static class StartUp
    {
        public static IServiceCollection AddDatabaseIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationIdentityDbContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionIdentity")))
                .AddTransient<ApplicationIdentityDbSeeder>();
            
            return services;
        }

        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services
                .AddTransient<ITokenService, TokenService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<IRoleService, RoleService>();

            return services;
        }
    }
}
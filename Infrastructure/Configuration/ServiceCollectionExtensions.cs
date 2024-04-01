using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuperMarket.Application.Services;
using SuperMarket.Application.Services.Identity;
using SuperMarket.Infrastructure.Context;
using SuperMarket.Infrastructure.Repositories;
using SuperMarket.Infrastructure.Services;
using SuperMarket.Infrastructure.Services.Identity;
using System.Reflection;

namespace SuperMarket.Infrastructure.Configuration
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

        public static IServiceCollection AddCatalogServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddTransient<IMarketRepository, MarketRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();

            // Creamos un acceso al contexto para poder obtener el userId activo para agregarlo 
            // automaticamente al crear un Order.
            // Solo se usa en el repositorio de Orders, los demás no lo requiren, por lo que no se
            // le asigna.
            services.AddTransient<IOrderRepository, OrderRepository>()
                .AddHttpContextAccessor().AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static void AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
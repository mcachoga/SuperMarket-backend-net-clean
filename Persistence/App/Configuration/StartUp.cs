using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuperMarket.Application.Services;
using SuperMarket.Application.Services.Contracts;
using SuperMarket.Infrastructure.Extensions.Caching;
using SuperMarket.Infrastructure.Framework.Security;
using SuperMarket.Infrastructure.Repositories;
using SuperMarket.Infrastructure.Services;
using SuperMarket.Persistence.Context;

namespace SuperMarket.Persistence.Configuration
{
    public static class StartUp
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                .AddTransient<ApplicationDbSeeeder>()
                .AddHttpContextAccessor().AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }

        public static IServiceCollection AddCatalogServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCustomConfigurationCache(configuration);

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
    }
}
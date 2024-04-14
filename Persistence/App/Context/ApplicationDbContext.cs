using Microsoft.EntityFrameworkCore;
using SuperMarket.Domain;
using SuperMarket.Domain.Abstractions;
using SuperMarket.Infrastructure.Extensions.EfCoreTrail;
using SuperMarket.Infrastructure.Framework.Security;

namespace SuperMarket.Persistence.Context
{
    public class ApplicationDbContext : AuditableContext
    {
        private readonly ICurrentUserService _currentUserService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) : base(options) 
        {
            _currentUserService = currentUserService;
        }


        public DbSet<Product> Products => Set<Product>();

        public DbSet<Market> Markets => Set<Market>();

        public DbSet<Order> Orders => Set<Order>();


        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model
                .GetEntityTypes().SelectMany(t => t.GetProperties()).Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        break;
                }
            }

            var auditEntries = OnBeforeSaveChanges(_currentUserService.UserId);
            var result = await base.SaveChangesAsync(cancellationToken);

            await OnAfterSaveChanges(auditEntries, cancellationToken);
            return result;
        }
    }
}
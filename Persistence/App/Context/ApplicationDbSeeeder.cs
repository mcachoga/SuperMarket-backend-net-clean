using Microsoft.EntityFrameworkCore;
using SuperMarket.Domain;

namespace SuperMarket.Persistence.Context
{
    public class ApplicationDbSeeeder
    {
        private readonly ApplicationDbContext _dbContext;

        public ApplicationDbSeeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ConfigureDatabaseAsync(bool useSeedData = true)
        {
            await CheckAndApplyPendingMigrationAsync();
            
            if (useSeedData)
            {
                await SeedCatalogDataAsync();
            }
        }

        private async Task CheckAndApplyPendingMigrationAsync()
        {
            if (_dbContext.Database.GetPendingMigrations().Any())
            {
                await _dbContext.Database.MigrateAsync();
            }
        }

        private async Task SeedCatalogDataAsync()
        {
            try
            {
                if (await _dbContext.Markets.AnyAsync() || await _dbContext.Products.AnyAsync()) return;

                await SeedMarketsAsync();
                await SeedProductsAsync();
            }
            catch (Exception exception)
            {
                throw new Exception("An error occurred while migrating or seeding the database.", exception);
            }
        }

        private async Task SeedMarketsAsync()
        {
            var markets = new List<string>()
            {
                "Mercadona", "Carrefour", "Alcampo", "El Jamón", "Día", "Cash García"
            };

            foreach (var market in markets)
            {
                await _dbContext.Markets.AddAsync( new Market { Name = market } );
            }

            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedProductsAsync()
        {
            var products = new List<Product>()
            {
                { new Product { Barcode = "84543221", Name = "Leche Pascual 1l", Description = "Brick 1l Vitaminas A,C,E" } },
                { new Product { Barcode = "84029274", Name = "Chocolate Nestle 120gr", Description = "Tableta chocolate con leche 120gr" } }
            };

            await _dbContext.Products.AddRangeAsync(products);

            await _dbContext.SaveChangesAsync();
        }
    }
}
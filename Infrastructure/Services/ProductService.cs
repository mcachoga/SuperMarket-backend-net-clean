using Microsoft.EntityFrameworkCore;
using SuperMarket.Application.Services;
using SuperMarket.Domain;
using SuperMarket.Infrastructure.Context;

namespace SuperMarket.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateProductAsync(Product market)
        {
            await _context.Products.AddAsync(market);
            await _context.SaveChangesAsync();
            return market;
        }

        public async Task<int> DeleteProductAsync(Product market)
        {
            _context.Products.Remove(market);
            await _context.SaveChangesAsync();
            return market.Id;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var productInDb = await _context.Products.FirstOrDefaultAsync(q => q.Id == id);
            return productInDb;
        }

        public async Task<List<Product>> GetProductListAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using SuperMarket.Application.Services;
using SuperMarket.Domain;

namespace SuperMarket.Infrastructure.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly IRepositoryAsync<Product> _repository;

        public ProductRepository(IRepositoryAsync<Product> repository)
        {
            _repository = repository;
        }

        public IQueryable<Product> Products => _repository.Entities;

        public async Task<Product> InsertAsync(Product product)
        {
            await _repository.AddAsync(product);
            return product;
        }

        public async Task<int> DeleteAsync(Product product)
        {
            await _repository.DeleteAsync(product);
            return product.Id;
        }

        public async Task<Product> GetByIdAsync(int productId)
        {
            return await _repository.Entities.Where(p => p.Id == productId).FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            await _repository.UpdateAsync(product);
            return product;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SuperMarket.Application.Services;
using SuperMarket.Application.Services.Contracts;
using SuperMarket.Domain;
using SuperMarket.Infrastructure.Extensions.Caching;
using SuperMarket.Persistence.Configuration.Cache;

namespace SuperMarket.Infrastructure.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly IRepositoryAsync<Product> _repository;
        private readonly IDistributedCache _distributedCache;

        public ProductRepository(IRepositoryAsync<Product> repository, IDistributedCache distributedCache)
        {
            _repository = repository;
            _distributedCache = distributedCache;
        }

        public IQueryable<Product> Products => _repository.Entities;


        public async Task<Product> GetByIdAsync(int productId)
        {
            string cacheKey = ProductCacheKeys.GetKey(productId);
            var entity = await _distributedCache.GetKeyAsync<Product>(cacheKey);

            if (entity == null)
            {
                entity = await _repository.Entities.Where(p => p.Id == productId).FirstOrDefaultAsync();
                await _distributedCache.SetKeyAsync(cacheKey, entity);
            }

            return entity;
        }

        public async Task<List<Product>> GetListAsync()
        {
            string cacheKey = ProductCacheKeys.ListKey;
            var list = await _distributedCache.GetKeyAsync<List<Product>>(cacheKey);

            if (list == null)
            {
                list = await _repository.Entities.ToListAsync();
                await _distributedCache.SetKeyAsync(cacheKey, list);
            }

            return list;
        }

        public async Task<Product> InsertAsync(Product product)
        {
            await _repository.AddAsync(product);

            await _distributedCache.RemoveKeyAsync(ProductCacheKeys.ListKey);

            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            await _repository.UpdateAsync(product);

            await _distributedCache.RemoveKeyAsync(ProductCacheKeys.ListKey);
            await _distributedCache.RemoveKeyAsync(ProductCacheKeys.GetKey(product.Id));

            return product;
        }

        public async Task<int> DeleteAsync(Product product)
        {
            await _repository.DeleteAsync(product);

            await _distributedCache.RemoveKeyAsync(ProductCacheKeys.ListKey);
            await _distributedCache.RemoveKeyAsync(ProductCacheKeys.GetKey(product.Id));

            return product.Id;
        }
    }
}
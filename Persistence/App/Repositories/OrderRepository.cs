using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SuperMarket.Application.Services;
using SuperMarket.Application.Services.Contracts;
using SuperMarket.Domain;
using SuperMarket.Infrastructure.Extensions.Caching;
using SuperMarket.Infrastructure.Framework.Security;
using SuperMarket.Persistence.Configuration.Cache;

namespace SuperMarket.Infrastructure.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IRepositoryAsync<Order> _repository;
        private readonly IDistributedCache _distributedCache;
        private readonly ICurrentUserService _currentUserService;

        public OrderRepository(IRepositoryAsync<Order> repository, IDistributedCache distributedCache, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _distributedCache = distributedCache;
            _currentUserService = currentUserService;
        }

        public IQueryable<Order> Orders => _repository.Entities;


        public async Task<Order> GetByIdAsync(int orderId)
        {
            string cacheKey = OrderCacheKeys.GetKey(orderId);
            var entity = await _distributedCache.GetKeyAsync<Order>(cacheKey);

            if (entity == null)
            {
                entity = await _repository.Entities.Where(p => p.Id == orderId).FirstOrDefaultAsync();
                await _distributedCache.SetKeyAsync(cacheKey, entity);
            }

            return entity;
        }

        public async Task<List<Order>> GetListAsync()
        {
            string cacheKey = ProductCacheKeys.ListKey;
            var list = await _distributedCache.GetKeyAsync<List<Order>>(cacheKey);

            if (list == null)
            {
                list = await _repository.Entities.ToListAsync();
                await _distributedCache.SetKeyAsync(cacheKey, list);
            }

            return list;
        }


        public async Task<Order> InsertAsync(Order order)
        {
            order.UserId = _currentUserService.UserId;
            
            await _repository.AddAsync(order);

            await _distributedCache.RemoveKeyAsync(OrderCacheKeys.ListKey);

            return order;
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            await _repository.UpdateAsync(order);

            await _distributedCache.RemoveKeyAsync(OrderCacheKeys.ListKey);
            await _distributedCache.RemoveKeyAsync(OrderCacheKeys.GetKey(order.Id));

            return order;
        }

        public async Task<int> DeleteAsync(Order order)
        {
            await _repository.DeleteAsync(order);

            await _distributedCache.RemoveKeyAsync(OrderCacheKeys.ListKey);
            await _distributedCache.RemoveKeyAsync(OrderCacheKeys.GetKey(order.Id));

            return order.Id;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SuperMarket.Application.Services;
using SuperMarket.Application.Services.Contracts;
using SuperMarket.Domain;
using SuperMarket.Infrastructure.Extensions.Caching;
using SuperMarket.Persistence.Configuration.Cache;

namespace SuperMarket.Infrastructure.Services
{
    public class MarketRepository : IMarketRepository
    {
        private readonly IRepositoryAsync<Market> _repository;
        private readonly IDistributedCache _distributedCache;

        public MarketRepository(IDistributedCache distributedCache, IRepositoryAsync<Market> repository)
        {
            _repository = repository;
            _distributedCache = distributedCache;
        }

        public IQueryable<Market> Markets => _repository.Entities;


        public async Task<Market> GetByIdAsync(int marketId)
        {
            string cacheKey = MarketCacheKeys.GetKey(marketId);
            var entity = await _distributedCache.GetKeyAsync<Market>(cacheKey);

            if (entity == null)
            {
                entity = await _repository.Entities.Where(p => p.Id == marketId).FirstOrDefaultAsync();
                await _distributedCache.SetKeyAsync(cacheKey, entity);
            }

            return entity;
        }

        public async Task<List<Market>> GetListAsync()
        {
            string cacheKey = MarketCacheKeys.ListKey;
            var list = await _distributedCache.GetKeyAsync<List<Market>>(cacheKey);

            if (list == null)
            {
                list = await _repository.Entities.ToListAsync();
                await _distributedCache.SetKeyAsync(cacheKey, list);
            }

            return list;
        }


        public async Task<Market> InsertAsync(Market market)
        {
            await _repository.AddAsync(market);
            
            await _distributedCache.RemoveKeyAsync(MarketCacheKeys.ListKey);
            
            return market;
        }

        public async Task<Market> UpdateAsync(Market market)
        {
            await _repository.UpdateAsync(market);
            
            await _distributedCache.RemoveKeyAsync(MarketCacheKeys.ListKey);
            await _distributedCache.RemoveKeyAsync(MarketCacheKeys.GetKey(market.Id));

            return market;
        }

        public async Task<int> DeleteAsync(Market market)
        {
            await _repository.DeleteAsync(market);

            await _distributedCache.RemoveKeyAsync(MarketCacheKeys.ListKey);
            await _distributedCache.RemoveKeyAsync(MarketCacheKeys.GetKey(market.Id));

            return market.Id;
        }
    }
}
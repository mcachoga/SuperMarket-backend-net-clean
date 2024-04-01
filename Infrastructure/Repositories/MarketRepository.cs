using Microsoft.EntityFrameworkCore;
using SuperMarket.Application.Services;
using SuperMarket.Domain;

namespace SuperMarket.Infrastructure.Services
{
    public class MarketRepository : IMarketRepository
    {
        private readonly IRepositoryAsync<Market> _repository;

        public MarketRepository(IRepositoryAsync<Market> repository)
        {
            _repository = repository;
        }

        public IQueryable<Market> Markets => _repository.Entities;

        public async Task<Market> InsertAsync(Market market)
        {
            await _repository.AddAsync(market);
            return market;
        }

        public async Task<int> DeleteAsync(Market market)
        {
            await _repository.DeleteAsync(market);
            return market.Id;
        }

        public async Task<Market> GetByIdAsync(int marketId)
        {
            return await _repository.Entities.Where(p => p.Id == marketId).FirstOrDefaultAsync();
        }

        public async Task<List<Market>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<Market> UpdateAsync(Market market)
        {
            await _repository.UpdateAsync(market);
            return market;
        }
    }
}
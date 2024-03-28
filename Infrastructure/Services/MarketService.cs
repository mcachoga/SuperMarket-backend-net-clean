using Microsoft.EntityFrameworkCore;
using SuperMarket.Application.Services;
using SuperMarket.Domain;
using SuperMarket.Infrastructure.Context;

namespace SuperMarket.Infrastructure.Services
{
    public class MarketService : IMarketService
    {
        private readonly ApplicationDbContext _context;

        public MarketService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Market> CreateMarketAsync(Market market)
        {
            await _context.Markets.AddAsync(market);
            await _context.SaveChangesAsync();
            return market;
        }

        public async Task<int> DeleteMarketAsync(Market market)
        {
            _context.Markets.Remove(market);
            await _context.SaveChangesAsync();
            return market.Id;
        }

        public async Task<Market> GetMarketByIdAsync(int id)
        {
            var marketInDb = await _context.Markets.FirstOrDefaultAsync(q => q.Id == id);
            return marketInDb;
        }

        public async Task<List<Market>> GetMarketListAsync()
        {
            return await _context.Markets.ToListAsync();
        }

        public async Task<Market> UpdateMarketAsync(Market market)
        {
            _context.Markets.Update(market);
            await _context.SaveChangesAsync();
            return market;
        }
    }
}
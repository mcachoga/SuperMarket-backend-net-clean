using SuperMarket.Domain;

namespace SuperMarket.Application.Services
{
    public interface IMarketService
    {
        Task<Market> CreateMarketAsync(Market market);

        Task<Market> UpdateMarketAsync(Market market);

        Task<int> DeleteMarketAsync(Market market);

        Task<Market> GetMarketByIdAsync(int id);

        Task<List<Market>> GetMarketListAsync();
    }
}
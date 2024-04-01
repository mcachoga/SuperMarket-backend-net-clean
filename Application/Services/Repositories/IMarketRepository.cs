using SuperMarket.Domain;

namespace SuperMarket.Application.Services
{
    public interface IMarketRepository
    {
        IQueryable<Market> Markets { get; }

        Task<List<Market>> GetListAsync();

        Task<Market> GetByIdAsync(int marketId);

        Task<Market> InsertAsync(Market marketId);

        Task<Market> UpdateAsync(Market marketId);

        Task<int> DeleteAsync(Market marketId);
    }
}
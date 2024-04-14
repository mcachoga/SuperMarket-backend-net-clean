using SuperMarket.Domain;

namespace SuperMarket.Application.Services.Contracts
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }

        Task<List<Order>> GetListAsync();

        Task<Order> GetByIdAsync(int orderId);

        Task<Order> InsertAsync(Order orderId);

        Task<Order> UpdateAsync(Order orderId);

        Task<int> DeleteAsync(Order orderId);
    }
}
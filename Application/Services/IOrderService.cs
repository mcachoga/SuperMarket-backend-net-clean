using SuperMarket.Domain;

namespace SuperMarket.Application.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order);

        Task<Order> UpdateOrderAsync(Order order);

        Task<int> DeleteOrderAsync(Order order);

        Task<Order> GetOrderByIdAsync(int id);

        Task<List<Order>> GetOrderListAsync();
    }
}
using Microsoft.EntityFrameworkCore;
using SuperMarket.Application.Services;
using SuperMarket.Application.Services.Identity;
using SuperMarket.Domain;
using SuperMarket.Infrastructure.Context;

namespace SuperMarket.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public OrderService(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            order.UserId = _currentUserService.UserId;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<int> DeleteOrderAsync(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return order.Id;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var orderInDb = await _context.Orders.Where(q => q.Id == id).FirstOrDefaultAsync();
            return orderInDb;
        }

        public async Task<List<Order>> GetOrderListAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
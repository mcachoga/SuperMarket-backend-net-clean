using Microsoft.EntityFrameworkCore;
using SuperMarket.Application.Services;
using SuperMarket.Application.Services.Identity;
using SuperMarket.Domain;

namespace SuperMarket.Infrastructure.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IRepositoryAsync<Order> _repository;
        private readonly ICurrentUserService _currentUserService;

        public OrderRepository(IRepositoryAsync<Order> repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public IQueryable<Order> Orders => _repository.Entities;

        public async Task<Order> InsertAsync(Order order)
        {
            order.UserId = _currentUserService.UserId;
            await _repository.AddAsync(order);
            return order;
        }

        public async Task<int> DeleteAsync(Order order)
        {
            await _repository.DeleteAsync(order);
            return order.Id;
        }

        public async Task<Order> GetByIdAsync(int orderId)
        {
            return await _repository.Entities.Where(p => p.Id == orderId).FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            await _repository.UpdateAsync(order);
            return order;
        }
    }
}
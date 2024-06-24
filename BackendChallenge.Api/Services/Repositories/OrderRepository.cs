using BackendChallenge.Api.Models.Entity;
using BackendChallenge.Api.Services.Database;
using BackendChallenge.Api.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendChallenge.Api.Services.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOrderAsync(OrderEntity order)
        {
            await _context.Order.AddAsync(order);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteOrderAsync(OrderEntity order)
        {
            _context.Order.Remove(order);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateOrderAsync(OrderEntity order)
        {
            _context.Order.Update(order);
            return await _context.SaveChangesAsync();
        }

        public async Task<OrderEntity> GetOrderAsync(int orderId)
        {
            return await _context.Order.FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IEnumerable<OrderEntity>> GetAllOrdersAsync()
        {
            return await _context.Order.ToListAsync();
        }

        public async Task<IEnumerable<OrderEntity>> GetClientOrdersAsync(int clientId)
        {
            return await _context.Order.Where(o => o.ClientId == clientId).ToListAsync();
        }
    }
}
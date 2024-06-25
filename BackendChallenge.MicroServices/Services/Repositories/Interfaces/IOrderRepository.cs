using BackendChallenge.Api.Models.Entity;

namespace BackendChallenge.Api.Services.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        
        Task<int> CreateOrderAsync(OrderEntity order);
        
        Task<int> DeleteOrderAsync(OrderEntity order);
        
        Task<int> UpdateOrderAsync(OrderEntity order);

        Task<OrderEntity> GetOrderAsync(int orderId);

        Task<IEnumerable<OrderEntity>> GetAllOrdersAsync();

        Task<IEnumerable<OrderEntity>> GetClientOrdersAsync(int clientId);
    }
}

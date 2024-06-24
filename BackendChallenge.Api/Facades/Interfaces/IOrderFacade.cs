using BackendChallenge.Api.Models.Entity;

namespace BackendChallenge.Api.Facades.Interfaces
{
    public interface IOrderFacade
    {
        void CreateOrderAsync(OrderEntity order);

        void UpdateOrderAsync(OrderEntity order);

        void DeleteOrderAsync(OrderEntity order);
    }
}

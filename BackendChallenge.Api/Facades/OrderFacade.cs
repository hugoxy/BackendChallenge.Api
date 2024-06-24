using BackendChallenge.Api.Facades.Interfaces;
using BackendChallenge.Api.Models.Entity;
using BackendChallenge.Api.Services.Repositories.Interfaces;

namespace BackendChallenge.Api.Facades
{
    public class OrderFacade : IOrderFacade
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<OrderFacade> _logger;

        public OrderFacade(IOrderRepository orderRepository, IClientRepository clientRepository, ILogger<OrderFacade> logger)
        {
            _orderRepository = orderRepository;
            _clientRepository = clientRepository;
            _logger = logger;
        }

        public void CreateOrderAsync(OrderEntity order)
        {
            
        }

        public void UpdateOrderAsync(OrderEntity order)
        {
            
        }

        public void DeleteOrderAsync(OrderEntity order)
        {
            
        }
    }
}

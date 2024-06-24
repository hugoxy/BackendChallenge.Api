using BackendChallenge.Api.Models.Entity;

namespace BackendChallenge.Api.Models.Requests.Order
{
    public class UpdateOrderRequest
    {
        public int OrderId { get; set; }
        public int ClientId { get; set; }
        public List<Products> Itens { get; set; }
    }
}
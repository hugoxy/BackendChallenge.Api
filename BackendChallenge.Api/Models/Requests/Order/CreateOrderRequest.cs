using BackendChallenge.Api.Models.Entity;
using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Requests.Order
{
    public class CreateOrderRequest
    {
        [Required]
        public int ClientId { get; set; }
        [Required]
        public List<ProductsEntity> Itens { get; set; }
    }
}
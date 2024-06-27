using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Requests.Order
{
    public class DeleteOrderRequest
    {
        [Required]
        public int OrderId { get; set; }
    }
}
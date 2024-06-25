using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Requests.Order
{
    internal class DeleteOrderRequest
    {
        [Required]
        public int OrderId { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Requests.Order
{
    public class ReadOrderRequest
    {
        [Required]
        public int OrderId { get; set; }
    }
}
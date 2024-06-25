using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Requests.Order
{
    internal class ReadOrderRequest
    {
        [Required]
        public int OrderId { get; set; }
    }
}
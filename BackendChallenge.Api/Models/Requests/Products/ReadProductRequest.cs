using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Requests.Products
{
    public class ReadProductRequest
    {
        [Required]
        public int ProductId { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Requests.Products
{
    public class DeleteProductRequest
    {
        [Required]
        public int ProductId { get; set; }
    }
}
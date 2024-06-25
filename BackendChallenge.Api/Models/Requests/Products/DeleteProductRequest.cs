using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Requests.Products
{
    internal class DeleteProductRequest
    {
        [Required]
        public int ProductId { get; set; }
    }
}
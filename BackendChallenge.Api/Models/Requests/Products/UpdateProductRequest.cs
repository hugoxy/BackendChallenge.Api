using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Requests.Products
{
    public class UpdateProductRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int Price { get; set; }
    }
}
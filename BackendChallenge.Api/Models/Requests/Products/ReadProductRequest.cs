using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Requests.Products
{
    internal class ReadProductRequest
    {
        [Required]
        public int ProductId { get; set; }
    }
}
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Entity
{
    public class ProductsEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public ProductsEntity(string productName, int quantity, int price)
        {
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }
    }
}
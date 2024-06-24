using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackendChallenge.Api.Models.Entity
{
    public class OrderItem
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Product { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public OrderItem(string product, int quantity, int price)
        {
            Product = product;
            Quantity = quantity;
            Price = price;
        }
    }
}
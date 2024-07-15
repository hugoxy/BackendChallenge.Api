using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendChallenge.Api.Models.Entity
{
    public class OrderEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [ForeignKey("ClientIdentity")]
        public int ClientId { get; set; }

        public double Total { get; set; }

        public List<ProductsEntity> Itens { get; set; }

        public OrderEntity()
        {
        }

        public OrderEntity(int orderId, int clientId, double total, List<ProductsEntity> itens)
        {
            OrderId = orderId;
            ClientId = clientId;
            Total = total;
            Itens = itens;
        }
    }
}

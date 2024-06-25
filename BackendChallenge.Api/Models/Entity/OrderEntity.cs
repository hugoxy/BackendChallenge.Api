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

    }
}

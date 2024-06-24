namespace BackendChallenge.Api.Models.Entity
{
    public class OrderEntity
    {
        public int OrderId { get; set; }

        public int ClientId { get; set; }

        public double Total { get; set; }

        public List<OrderItem> Itens { get; set; }


    }
}

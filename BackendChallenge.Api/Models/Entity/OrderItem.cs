namespace BackendChallenge.Api.Models.Entity
{
    public class OrderItem
    {

        public string Product { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public OrderItem(string product, int quantity, int price)
        {
            Product = product;
            Quantity = quantity;
            Price = price;
        }

        public override bool Equals(object obj)
        {
            return obj is OrderItem item &&
                   Product == item.Product &&
                   Quantity == item.Quantity &&
                   Price == item.Price;
        }
    }
}
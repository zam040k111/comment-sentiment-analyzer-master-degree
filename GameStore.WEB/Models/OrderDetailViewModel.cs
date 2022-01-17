namespace GameStore.WEB.Models
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; }

        public int? ProductId { get; set; }

        public string ProductKey { get; set; }

        public GameViewModel Product { get; set; }

        public decimal Price => Product.Price * Quantity;

        public short Quantity { get; set; } = 1;

        public float Discount { get; set; }

        public int? OrderId { get; set; }

        public OrderViewModel Order { get; set; }
    }
}

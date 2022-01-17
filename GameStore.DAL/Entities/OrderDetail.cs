namespace GameStore.DAL.Entities
{
    public class OrderDetail : SoftDeletable
    {
        public int Id { get; set; }

        public int? ProductId { get; set; }

        public string ProductKey { get; set; }

        public Game Product { get; set; }

        public decimal Price { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }

        public int? OrderId { get; set; }

        public Order Order { get; set; }
    }
}

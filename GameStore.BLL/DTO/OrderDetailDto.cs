namespace GameStore.BLL.DTO
{
    public class OrderDetailDto
    {
        public int Id { get; set; }

        public int? ProductId { get; set; }

        public string ProductKey { get; set; }

        public GameDto Product { get; set; }

        public decimal Price { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }

        public int? OrderId { get; set; }

        public OrderDto Order { get; set; }
    }
}


namespace GameStore.DAL.Northwind.Entities
{
    public class OrderDetails : MongoModel
    {
        public int ProductId { get; set; }

        public double Price { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }
        
        public int OrderId { get; set; }
    }
}
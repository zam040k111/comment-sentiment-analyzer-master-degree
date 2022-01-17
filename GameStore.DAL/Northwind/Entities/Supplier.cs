
namespace GameStore.DAL.Northwind.Entities
{
    public class Supplier : MongoModel
    {
        public int SupplierId { get; set; }

        public string CompanyName { get; set; }

        public string Description { get; set; }

        public string HomePage { get; set; }
    }
}
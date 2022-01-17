
namespace GameStore.DAL.Entities
{
    public class Publisher : SoftDeletable
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string Description { get; set; }

        public string HomePage { get; set; }
    }
}

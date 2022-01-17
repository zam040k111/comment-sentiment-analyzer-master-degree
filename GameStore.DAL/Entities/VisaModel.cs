
namespace GameStore.DAL.Entities
{
    public class VisaModel : SoftDeletable
    {
        public int Id { get; set; }

        public string HolderName { get; set; }

        public string CardNumber { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int? Cvc { get; set; }

        public int? OrderId { get; set; }
    }
}

namespace GameStore.DAL.Northwind.Entities
{
    public class Log : MongoModel
    {
        public string Date { get; set; }

        public string Operation { get; set; }

        public string EntityType { get; set; }

        public string OldEntity { get; set; }

        public string NewEntity { get; set; }
    }
}
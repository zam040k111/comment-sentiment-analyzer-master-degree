
namespace GameStore.DAL.Entities
{
    public class Comment : SoftDeletable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Body { get; set; }

        public int? GameId { get; set; }

        public string GameKey { get; set; }

        public Game Game { get; set; }

        public int? ParentCommentId { get; set; }

        public Comment ParentComment { get; set; }

        public bool IsQuote { get; set; }

        public float Score { get; set; }
    }
}

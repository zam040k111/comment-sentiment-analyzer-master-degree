
namespace GameStore.BLL.DTO
{
    public class CommentDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Body { get; set; }

        public int? GameId { get; set; }

        public string GameKey { get; set; }

        public int? ParentCommentId { get; set; }

        public CommentDto ParentComment { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsQuote { get; set; }

        public float Score { get; set; }
    }
}

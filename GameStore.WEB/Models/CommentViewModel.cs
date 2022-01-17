using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.WEB.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        private string _body;

        [Required]
        public string Body
        {
            set => _body = value;
            get => IsDeleted ? "<<Deleted>>" : _body;
        }

        public int? GameId { get; set; }

        public string GameKey { get; set; }

        public int? ParentCommentId { get; set; }

        public CommentViewModel ParentComment { get; set; }

        public bool IsQuote { get; set; }

        public float Score { get; set; }

        public bool IsDeleted { get; set; }

        public List<CommentViewModel> Comments { get; set; }

        public List<CommentViewModel> Children { get; set; }
    }
}
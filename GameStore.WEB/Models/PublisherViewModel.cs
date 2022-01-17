
using System.ComponentModel.DataAnnotations;

namespace GameStore.WEB.Models
{
    public class PublisherViewModel
    {
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string HomePage { get; set; }

    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.WEB.Models
{
    public class PlatformTypeViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        public List<int?> GamePlatformTypesId { get; set; }
    }
}

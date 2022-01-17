using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameStore.WEB.Models
{
    public class GameViewModel
    {
        public int Id { get; set; }

        public string Key { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "The price can not be less than 0.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "The price can be like 9.99 or 10 or 5.2")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, short.MaxValue, ErrorMessage = "The unit of stock can not be less than 0.")]
        public short UnitsInStock { get; set; }

        [Required]
        public bool Discontinued { get; set; }

        public DateTime Published { get; set; }

        public int Viewed { get; set; }

        [Required]
        public int? PublisherId { get; set; }

        public PublisherViewModel Publisher { get; set; }

        public string Image { get; set; }

        public string SmallImage { get; set; }

        public float Score { get; set; }

        public List<int?> GameGenresId { get; set; }

        public List<GameGenreViewModel> GameGenres { get; set; }

        [Required]
        [MinLength(1)]
        public List<int?> GamePlatformTypesId { get; set; }

        public List<GamePlatformTypeViewModel> GamePlatformTypes { get; set; }

        public List<SelectListItem> GenreListItems { get; set; }

        public List<SelectListItem> PublisherListItems { get; set; }

        public List<SelectListItem> PlatformTypesItems { get; set; }
    }
}

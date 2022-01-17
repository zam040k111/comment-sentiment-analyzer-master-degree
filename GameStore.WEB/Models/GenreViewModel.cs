
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameStore.WEB.Models
{
    public class GenreViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int? ParentGenreId { get; set; }

        public GenreViewModel ParentGenre { get; set; }

        public List<int?> GameGenresId { get; set; }

        public GenreViewModel Child { get; set; }

        public List<GenreViewModel> Genres { get; set; }

        public List<GameGenreViewModel> GameGenres { get; set; }

        public List<SelectListItem> GenreParentList { get; set; }
    }
}

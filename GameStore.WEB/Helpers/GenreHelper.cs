using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.WEB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameStore.WEB.Helpers
{
    public static class GenreHelper
    {
        public static List<SelectListItem> CreateGenreList(List<GenreDto> genres)
        {
            var listItems = new List<SelectListItem>();

            foreach (var genre in genres)
            {
                if (genre.ParentGenreId == null)
                {
                    var group = new SelectListGroup() { Name = genre.Name };
                    foreach (var genreGroup in genres)
                    {
                        if (genreGroup.ParentGenreId == genre.Id)
                        {
                            listItems.Add(new SelectListItem
                            { Value = genreGroup.Id.ToString(), Text = genreGroup.Name, Group = group });
                        }
                    }

                    if (!genres.Select(i => i.ParentGenreId).Contains(genre.Id))
                    {
                        listItems.Add(new SelectListItem { Value = genre.Id.ToString(), Text = genre.Name });
                    }
                }
            }

            return listItems;
        }

        public static List<SelectListItem> CreateListWithoutGroups(List<GenreDto> genres) =>
            new List<SelectListItem>(genres.Select(genre => new SelectListItem {Text = genre.Name, Value = genre.Id.ToString()}));

        public static void InitializeSelectLists(GenreViewModel genre, IGenreService genreService, IMapper mapper)
        {
            genre.Genres = mapper.Map<List<GenreViewModel>>(genreService.GetAll());
            genre.GenreParentList = CreateGenreList(genreService.GetAll());
        }
    }
}

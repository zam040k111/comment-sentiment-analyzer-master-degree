using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.Interfaces;
using GameStore.WEB.Helpers;
using GameStore.WEB.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameStore.WEB.Extensions
{
    public static class ModelExtensions
    {
        public static void InitSelectLists(
            this GameViewModel model,
            IGenreService genreService,
            IPublisherService publisherService,
            IPlatformTypeService platformTypeService)
        {
            model.GenreListItems = GenreHelper.CreateGenreList(genreService.GetAll());

            if (model.GameGenresId != null)
            {
                model.GenreListItems
                    .Where(genre => model.GameGenresId.Any(id => genre.Value == id?.ToString()))
                    .ToList()
                    .ForEach(item => item.Selected = true);
            }

            model.PublisherListItems = new List<SelectListItem>(publisherService.GetAll()
                .Select(publisher => new SelectListItem { Text = publisher.CompanyName, Value = publisher.Id.ToString() }));

            if (model.PublisherId != null)
            {
                var selected = model.PublisherListItems
                    .FirstOrDefault(item => item.Value == model.PublisherId.ToString());

                if (selected != null)
                {
                    selected.Selected = true;
                }
            }

            model.PlatformTypesItems = new List<SelectListItem>(platformTypeService.GetAll()
                .Select(type => new SelectListItem { Text = type.Type, Value = type.Id.ToString() }));

            if (model.GamePlatformTypesId != null)
            {
                model.PlatformTypesItems
                    .Where(genre => model.GamePlatformTypesId.Any(id => genre.Value == id?.ToString()))
                    .ToList()
                    .ForEach(item => item.Selected = true);
            }
        }
    }
}

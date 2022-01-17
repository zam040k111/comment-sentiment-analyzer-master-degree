using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.WEB.Helpers;
using GameStore.WEB.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WEB.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;

        public GenreController(
            IGenreService genreService,
            IGameService gameService,
            IMapper mapper)
        {
            _genreService = genreService;
            _gameService = gameService;
            _mapper = mapper;
        }

        [HttpGet("genres")]
        public ActionResult<GenreViewModel> GetGenres()
        {
            var genres = new GenreViewModel();
            GenreHelper.InitializeSelectLists(genres, _genreService, _mapper);

            return View(genres);
        }

        [HttpGet]
        public ActionResult<GenreViewModel> Create()
        {
            var genre = new GenreViewModel{GenreParentList = GenreHelper.CreateListWithoutGroups(_genreService.GetAll())};

            return View(genre);
        }

        [HttpPost]
        public ActionResult<GenreViewModel> Create(GenreViewModel genreViewModel)
        {
            genreViewModel.GenreParentList = GenreHelper.CreateListWithoutGroups(_genreService.GetAll());
            
            if (!ModelState.IsValid)
            {
                return View(genreViewModel);
            }

            var result = _genreService.Add(_mapper.Map<GenreDto>(genreViewModel));

            if (result.IsValid)
            {
                return Redirect("~/genres");
            }

            result.Errors.ToList().ForEach(error => ModelState.AddModelError(error.Key, error.Value));

            return View(genreViewModel);
        }

        [HttpGet("genre/{id?}/update")]
        public ActionResult<GenreViewModel> Update(int id)
        {
            var genre = _mapper.Map<GenreViewModel>(_genreService.GetById(id));
            genre.GenreParentList = GenreHelper.CreateListWithoutGroups(_genreService.GetAll());

            return View(genre);
        }

        [HttpPost("genre/update")]
        public ActionResult<GenreViewModel> Update(GenreViewModel genreViewModel)
        {
            genreViewModel.GenreParentList = GenreHelper.CreateListWithoutGroups(_genreService.GetAll());

            if (!ModelState.IsValid)
            {
                return View(genreViewModel);
            }

            var result = _genreService.Update(_mapper.Map<GenreDto>(genreViewModel));

            if (result.IsValid)
            {
                return Redirect("~/genres");
            }

            result.Errors.ToList().ForEach(error => ModelState.AddModelError(error.Key, error.Value));
                
            return View(genreViewModel);
        }

        [HttpGet("genre/{id?}/remove")]
        public RedirectResult Remove(int id)
        {
            _genreService.Delete(id);

            return Redirect("~/genres");
        }

        [HttpGet]
        public string CheckUpdateIsAllowed(int id)
        {
            var games = _gameService.GetByGenre(id);

            return string.Join("\", \"", games.Select(game => game.Key));
        }
    }
}

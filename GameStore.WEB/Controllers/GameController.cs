using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.WEB.Extensions;
using GameStore.WEB.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GameStore.WEB.Models;
using GameStore.WEB.Services.Models;

namespace GameStore.WEB.Controllers
{
    public class GameController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly IGameService _gameService;
        private readonly ICommentService _commentService;
        private readonly IGenreService _genreService;
        private readonly IPlatformTypeService _platformTypeService;
        private readonly IPublisherService _publisherService;

        public GameController(
            IGameService gameService,
            ICommentService commentService,
            IGenreService genreService,
            IPlatformTypeService platformTypeService,
            IPublisherService publisherService,
            IMapper mapper,
            IFileService fileService)
        {
            _gameService = gameService;
            _commentService = commentService;
            _genreService = genreService;
            _platformTypeService = platformTypeService;
            _mapper = mapper;
            _fileService = fileService;
            _publisherService = publisherService;
        }

        [HttpGet("game/{key}")]
        public ActionResult<GameViewModel> GetGame(string key)
        {
            var result = _mapper.Map<GameViewModel>(_gameService.GetByKeyFromBothDb(key));

            return View(result);
        }

        [HttpGet("games")]
        public ActionResult<GameFilterViewModel> GetGames()
        {
            var filtered = _mapper.Map<GameFilterViewModel>(_gameService.GetAllFromBothDb(10,1));
            filtered.GameItems.InitSelectLists(_genreService, _publisherService, _platformTypeService);

            return View(filtered);
        }

        [HttpGet("gamesByFilter")]
        public ActionResult<GameFilterViewModel> GetGamesByFilter(GameFilterViewModel model)
        {
            var filtered = _mapper.Map<GameFilterViewModel>(_gameService.ApplyFilterBothBd(_mapper.Map<GameFilterDto>(model)));
            filtered.GameItems.InitSelectLists(_genreService, _publisherService, _platformTypeService);

            return View("GetGames", filtered);
        }

        [HttpGet("game/create")]
        public ActionResult<GameViewModel> Create()
        {
            var model = new GameViewModel();
            model.InitSelectLists(_genreService, _publisherService, _platformTypeService);
        
            return View(model);
        }

        [HttpPost("game/create")]
        public ActionResult<GameViewModel> Create(GameViewModel gameViewModel)
        {
            gameViewModel.InitSelectLists(_genreService, _publisherService, _platformTypeService);

            if (!ModelState.IsValid)
            {
                return View(gameViewModel);
            }

            Request.Form.Files["Image"].FileToBase64(i => i.Image, gameViewModel);
            Request.Form.Files["SmallImage"].FileToBase64(i => i.SmallImage, gameViewModel);

            var result = _gameService.Add(_mapper.Map<GameDto>(gameViewModel));

            if (result.IsValid)
            {
                return Redirect("~/games");
            }

            result.Errors.ToList().ForEach(error => ModelState.AddModelError(error.Key, error.Value));

            return View(gameViewModel);
        }

        [HttpGet("game/{id?}/update")]
        public ActionResult<GameViewModel> Update(int id)
        {
            var model = _mapper.Map<GameViewModel>(_gameService.GetById(id));
            model.InitSelectLists(_genreService, _publisherService, _platformTypeService);

            return View(model);
        }

        [HttpPost("game/update")]
        public ActionResult<GameViewModel> Update(GameViewModel gameViewModel)
        {
            gameViewModel.InitSelectLists(_genreService, _publisherService, _platformTypeService);

            if (!ModelState.IsValid)
            {
                return View(gameViewModel);
            }

            Request.Form.Files["Image"].FileToBase64(i => i.Image, gameViewModel);
            Request.Form.Files["SmallImage"].FileToBase64(i => i.SmallImage, gameViewModel);

            var result = _gameService.Update(_mapper.Map<GameDto>(gameViewModel));

            if (result.IsValid)
            {
                return Redirect(result.Value.Key);
            }

            result.Errors.ToList().ForEach(error => ModelState.AddModelError(error.Key, error.Value));

            return View(gameViewModel);
        }

        [HttpGet("game/{id?}/remove")]
        public RedirectResult Remove(int id)
        {
            _gameService.Delete(id);

            return Redirect("~/games");
        }

        [HttpPost("game/{gameKey}/{gameId?}/createComment")]
        public ActionResult CreateComment(CommentViewModel commentViewModel)
        {
            if (commentViewModel.ParentCommentId != null)
            {
                commentViewModel.ParentComment = _mapper.Map<CommentViewModel>
                    (_commentService.GetById(commentViewModel.ParentCommentId.Value));
            }

            if (!ModelState.IsValid)
            {
                commentViewModel.Comments = _mapper.Map<List<CommentViewModel>>
                    (_commentService.GetAllByGameKey(commentViewModel.GameKey, true));

                return View("Comments", commentViewModel);
            }

            _commentService.Add(_mapper.Map<CommentDto>(commentViewModel));

            return Redirect("Comments");
        }

        [HttpGet("game/{key}/{id?}/comments")]
        public ActionResult<CommentViewModel> Comments(string key)
        {
            ViewBag.Game = _mapper.Map<GameViewModel>(_gameService.GetByKey(key));
            var model = new CommentViewModel
            { Comments = _mapper.Map<List<CommentViewModel>>(_commentService.GetAllByGameKey(key, true)) };

            return View(model);
        }

        [HttpGet("game/{gameKey}/{gameId?}/removeComment/{id}")]
        public ActionResult<CommentViewModel> RemoveComment(int id, string gameKey, int gameId)
        {
            _commentService.Delete(id);

            return Redirect($"~/game/{gameKey}/{gameId}/Comments");
        }

        [HttpGet("game/{key}/download")]
        public ActionResult Download(string key) => File(_fileService.Download(key, FileType.Bin), FileType.Bin);
    }
}

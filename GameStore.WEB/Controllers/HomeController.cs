using AutoMapper;
using GameStore.BLL.Interfaces;
using GameStore.WEB.Models;
using GameStore.WEB.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GameStore.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGameService _gameService;

        public HomeController(IMapper mapper, IGameService gameService)
        {
            _mapper = mapper;
            _gameService = gameService;
        }

        public ActionResult Index()
        {
            var model = _mapper.Map<List<GameViewModel>>(_gameService.GetBest(3));

            return View(model);
        }

        [Route("/Home/HandleError")]
        public ActionResult<ErrorDetails> HandleError()
        {
            return View("Error", Error._404);
        }
    }
}

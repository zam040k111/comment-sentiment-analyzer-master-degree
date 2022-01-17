using AutoMapper;
using GameStore.BLL.Interfaces;
using GameStore.WEB.Interfaces;
using GameStore.WEB.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WEB.Controllers
{
    public class BasketController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public BasketController(
            IGameService gameService,
            ICartService cartService,
            IMapper mapper)
        {
            _gameService = gameService;
            _cartService = cartService;
            _mapper = mapper;
        }

        [HttpGet("game/{key}/buy")]
        public RedirectResult Buy(string key)
        {
            var game = _gameService.GetByKeyFromBothDb(key);
            HttpContext.Session = _cartService.Add(game, HttpContext.Session);

            return Redirect("~/game/" + key);
        }

        [HttpGet("basket/remove/{gameKey}")]
        public RedirectResult RemoveFromBasket(string gameKey)
        {
            HttpContext.Session = _cartService.Remove(gameKey, HttpContext.Session);

            return Redirect("~/basket");
        }

        [HttpGet("basket")]
        public ActionResult<OrderViewModel> Basket()
        {
            var cartList = _mapper.Map<OrderViewModel>(_cartService.GetAll(HttpContext.Session));

            return View(cartList);
        }
    }
}

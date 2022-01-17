using System;
using Microsoft.AspNetCore.Mvc;
using GameStore.BLL.Interfaces;

namespace GameStore.WEB.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) => _userService = userService;

        [HttpGet("game/{key}/ban/{userName}")]
        public ActionResult Ban() => View();

        [HttpPost("game/{key}/ban/{userName}")]
        public ActionResult Ban(string key, string userName, DateTime until)
        {
            var user = _userService.GetByName(userName);
            _userService.Ban(user, until);

            return Redirect($"~/game/{key}/comments");
        }
    }
}

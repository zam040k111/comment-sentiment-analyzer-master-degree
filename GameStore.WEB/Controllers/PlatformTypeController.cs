using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.WEB.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WEB.Controllers
{
    public class PlatformTypeController : Controller
    {
        private readonly IPlatformTypeService _platformTypeService;
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;

        public PlatformTypeController(IPlatformTypeService platformTypeService, IGameService gameService, IMapper mapper)
        {
            _platformTypeService = platformTypeService;
            _gameService = gameService;
            _mapper = mapper;
        }

        [HttpGet("platformTypes")]
        public ActionResult<List<PlatformTypeViewModel>> GetPlatformTypes()
        {
            var platformTypes = _mapper.Map<List<PlatformTypeViewModel>>(_platformTypeService.GetAll());

            return View(platformTypes);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult<PlatformTypeViewModel> Create(PlatformTypeViewModel platformTypeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(platformTypeViewModel);
            }

            var result = _platformTypeService.Add(_mapper.Map<PlatformTypeDto>(platformTypeViewModel));

            if (result.IsValid)
            {
                return Redirect("~/platformTypes");
            }

            result.Errors.ToList().ForEach(error => ModelState.AddModelError(error.Key, error.Value));
                
            return View(platformTypeViewModel);
        }

        [HttpGet("platformType/{id?}/update")]
        public ActionResult<PlatformTypeViewModel> Update(int id)
        {
            var result = _mapper.Map<PlatformTypeViewModel>(_platformTypeService.GetById(id));

            return View(result);
        }

        [HttpPost("platformType/update")]
        public ActionResult<PlatformTypeViewModel> Update(PlatformTypeViewModel platformTypeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(platformTypeViewModel);
            }

            var result = _platformTypeService.Update(_mapper.Map<PlatformTypeDto>(platformTypeViewModel));

            if (result.IsValid)
            {
                return Redirect("~/platformTypes");
            }

            result.Errors.ToList().ForEach(error => ModelState.AddModelError(error.Key, error.Value));
            
            return View(platformTypeViewModel);
        }

        [HttpGet("platformType/{id?}/remove")]
        public RedirectResult Remove(int id)
        {
            _platformTypeService.Delete(id);

            return Redirect("~/platformTypes");
        }

        [HttpGet]
        public string CheckUpdateIsAllowed(int id)
        {
            var games = _gameService.GetByPlatformType(id);

            return string.Join("\", \"", games.Select(game => game.Key));
        }
    }
}

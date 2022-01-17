using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.WEB.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WEB.Controllers
{
    public class PublisherController : Controller
    {
        private readonly IPublisherService _publisherService;
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;

        public PublisherController(IPublisherService publisherService, IGameService gameService, IMapper mapper)
        {
            _publisherService = publisherService;
            _gameService = gameService;
            _mapper = mapper;
        }

        [HttpGet("publishers")]
        public ActionResult<List<PublisherViewModel>> GetPublishers()
        {
            var publishers = _mapper.Map<List<PublisherViewModel>>(_publisherService.GetAll());

            return View(publishers);
        }

        [HttpGet("publisher/{id?}")]
        public ActionResult<PublisherViewModel> GetPublisher(int id)
        {
            var publisher = _mapper.Map<PublisherViewModel>(_publisherService.GetById(id));

            return View(publisher);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult<PublisherViewModel> Create(PublisherViewModel publisherViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(publisherViewModel);
            }

            var result = _publisherService.Add(_mapper.Map<PublisherDto>(publisherViewModel));

            if (result.IsValid)
            {
                return Redirect("~/publishers");
            }

            result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Key, e.Value));
            
            return View(publisherViewModel);
        }

        [HttpGet("publisher/{id?}/update")]
        public ActionResult<PublisherViewModel> Update(int id)
        {
            var publisher = _mapper.Map<PublisherViewModel>(_publisherService.GetById(id));

            return View(publisher);
        }

        [HttpPost("publisher/update")]
        public ActionResult<PublisherViewModel> Update(PublisherViewModel publisherViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(publisherViewModel);
            }

            var result = _publisherService.Update(_mapper.Map<PublisherDto>(publisherViewModel));

            if (result.IsValid)
            {
                return Redirect("~/publishers");
            }

            result.Errors.ToList().ForEach(error => ModelState.AddModelError(error.Key, error.Value));
            
            return View(publisherViewModel);
        }

        [HttpGet("publisher/{id?}/remove")]
        public RedirectResult Remove(int id)
        {
            _publisherService.Delete(id);

            return Redirect("~/publishers");
        }

        [HttpGet]
        public string CheckUpdateIsAllowed(int id)
        {
            var games = _gameService.GetByPublisher(id);

            return string.Join("\", \"", games.Select(game => game.Key));
        }
    }
}

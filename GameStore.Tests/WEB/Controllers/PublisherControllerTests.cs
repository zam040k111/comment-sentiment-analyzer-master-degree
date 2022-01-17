using System.Collections.Generic;
using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services.Validation;
using GameStore.WEB.Controllers;
using GameStore.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GameStore.Tests.WEB.Controllers
{
    public class PublisherControllerTests : WebTests
    {
        private readonly Mock<IPublisherService> _publisherService;
        private readonly Mock<IGameService> _gameService;

        public PublisherControllerTests()
        {
            _publisherService = new Mock<IPublisherService>();
            _gameService = new Mock<IGameService>();
        }

        [Fact]
        public void GetPublishers_Always_ExpectViewResult()
        {
            // Arrange
            _publisherService.Setup(i => i.GetAll(false)).Returns(new List<PublisherDto>());
            var publisherController = new PublisherController(_publisherService.Object, _gameService.Object, Mapper);

            // Act
            var result = publisherController.GetPublishers();

            // Assert
            Assert.IsType<ViewResult>(result.Result);
        }

        [Fact]
        public void GetPublisher_Always_ExpectViewResult()
        {
            // Arrange
            const int id = 1;
            _publisherService.Setup(i => i.GetById(id)).Returns(new PublisherDto());
            var publisherController = new PublisherController(_publisherService.Object, _gameService.Object, Mapper);

            // Act
            var result = publisherController.GetPublisher(id);

            // Assert
            Assert.IsType<ViewResult>(result.Result);
        }

        [Fact]
        public void Create_WhenModelValid_ExpectRedirectResultToPublishers()
        {
            // Arrange
            _publisherService.Setup(i => i.Add(It.IsAny<PublisherDto>())).Returns(new Result<PublisherDto>());
            var publisherController = new PublisherController(_publisherService.Object, _gameService.Object, Mapper);

            // Act
            var result = publisherController.Create(Mapper.Map<PublisherViewModel>(CreatePublisherDto()));

            // Assert
            var viewResult = Assert.IsType<RedirectResult>(result.Result);
            Assert.Equal("~/publishers", viewResult.Url);
        }

        [Fact]
        public void Create_WhenModelNotValidFromServerSide_ExpectViewResult()
        {
            // Arrange
            var resultWithError = new Result<PublisherDto>();
            resultWithError.Errors.Add("Type", "some message");
            _publisherService.Setup(i => i.Add(It.IsAny<PublisherDto>())).Returns(resultWithError);
            var publisherController = new PublisherController(_publisherService.Object, _gameService.Object, Mapper);

            // Act
            var result = publisherController.Create(Mapper.Map<PublisherViewModel>(CreatePublisherDto()));

            // Assert
            Assert.IsType<ViewResult>(result.Result);
        }

        [Fact]
        public void Create_WhenModelNotValidFromUserSide_ExpectViewResult()
        {
            // Arrange
            _publisherService.Setup(i => i.Add(It.IsAny<PublisherDto>())).Returns(new Result<PublisherDto>());
            var publisherController = new PublisherController(_publisherService.Object, _gameService.Object, Mapper);
            publisherController.ModelState.AddModelError("key", "some message");

            // Act
            var result = publisherController.Create(Mapper.Map<PublisherViewModel>(CreatePublisherDto()));

            // Assert
            Assert.IsType<ViewResult>(result.Result);
        }

        [Fact]
        public void Update_WhenModelValid_ExpectRedirectResultToPublishers()
        {
            // Arrange
            _publisherService.Setup(i => i.Update(It.IsAny<PublisherDto>())).Returns(new Result<PublisherDto>());
            var publisherController = new PublisherController(_publisherService.Object, _gameService.Object, Mapper);

            // Act
            var result = publisherController.Update(Mapper.Map<PublisherViewModel>(CreatePublisherDto()));

            // Assert
            var viewResult = Assert.IsType<RedirectResult>(result.Result);
            Assert.Equal("~/publishers", viewResult.Url);
        }

        [Fact]
        public void Update_WhenModelNotValidFromServerSide_ExpectViewResult()
        {
            // Arrange
            var resultWithError = new Result<PublisherDto>();
            resultWithError.Errors.Add("Type", "some message");
            _publisherService.Setup(i => i.Update(It.IsAny<PublisherDto>())).Returns(resultWithError);
            var publisherController = new PublisherController(_publisherService.Object, _gameService.Object, Mapper);

            // Act
            var result = publisherController.Update(Mapper.Map<PublisherViewModel>(CreatePublisherDto()));

            // Assert
            Assert.IsType<ViewResult>(result.Result);
        }

        [Fact]
        public void Update_WhenModelNotValidFromUserSide_ExpectViewResult()
        {
            // Arrange
            _publisherService.Setup(i => i.Update(It.IsAny<PublisherDto>())).Returns(new Result<PublisherDto>());
            var publisherController = new PublisherController(_publisherService.Object, _gameService.Object, Mapper);
            publisherController.ModelState.AddModelError("key", "some message");

            // Act
            var result = publisherController.Update(Mapper.Map<PublisherViewModel>(CreatePublisherDto()));

            // Assert
            Assert.IsType<ViewResult>(result.Result);
        }

        [Fact]
        public void Remove_WhenIdValid_ExpectRedirectResultToPublishers()
        {
            // Arrange
            const int id = 1;
            _publisherService.Setup(i => i.Delete(id));
            _publisherService.Setup(i => i.GetById(id)).Returns(new PublisherDto());
            var publisherController = new PublisherController(_publisherService.Object, _gameService.Object, Mapper);

            // Act
            var result = publisherController.Remove(id);

            // Assert
            var viewResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("~/publishers", viewResult.Url);
        }
    }
}

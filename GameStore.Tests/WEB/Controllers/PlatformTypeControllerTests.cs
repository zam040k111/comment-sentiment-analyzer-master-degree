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
    public class PlatformTypeControllerTests : WebTests
    {
        private readonly Mock<IPlatformTypeService> _platformTypeService;
        private readonly Mock<IGameService> _gameService;

        public PlatformTypeControllerTests()
        {
            _platformTypeService = new Mock<IPlatformTypeService>();
            _gameService = new Mock<IGameService>();
        }

        [Fact]
        public void GetPlatformTypes_Always_ExpectViewResult()
        {
            // Arrange
            _platformTypeService.Setup(i => i.GetAll(false)).Returns(new List<PlatformTypeDto>());
            var platformTypeController = new PlatformTypeController(_platformTypeService.Object, _gameService.Object, Mapper);

            // Act
            var result = platformTypeController.GetPlatformTypes();

            // Assert
            Assert.IsType<ViewResult>(result.Result);
        }

        [Fact]
        public void Create_WhenModelValid_ExpectRedirectResultToPlatformTypes()
        {
            // Arrange
            _platformTypeService.Setup(i => i.Add(It.IsAny<PlatformTypeDto>())).Returns(new Result<PlatformTypeDto>());
            var platformTypeController = new PlatformTypeController(_platformTypeService.Object, _gameService.Object, Mapper);

            // Act
            var result = platformTypeController.Create(Mapper.Map<PlatformTypeViewModel>(CreatePlatformTypeDto()));

            // Assert
            var viewResult = Assert.IsType<RedirectResult>(result.Result);
            Assert.Equal("~/platformTypes", viewResult.Url);
        }

        [Fact]
        public void Create_WhenModelNotValidFromServerSide_ExpectViewResult()
        {
            // Arrange
            var resultWithError = new Result<PlatformTypeDto>();
            resultWithError.Errors.Add("Type","some message");
            _platformTypeService.Setup(i => i.Add(It.IsAny<PlatformTypeDto>())).Returns(resultWithError);
            var platformTypeController = new PlatformTypeController(_platformTypeService.Object, _gameService.Object, Mapper);

            // Act
            var result = platformTypeController.Create(Mapper.Map<PlatformTypeViewModel>(CreatePlatformTypeDto()));

            // Assert
            Assert.IsType<ViewResult>(result.Result);
        }

        [Fact]
        public void Create_WhenModelNotValidFromUserSide_ExpectViewResult()
        {
            // Arrange
            _platformTypeService.Setup(i => i.Add(It.IsAny<PlatformTypeDto>())).Returns(new Result<PlatformTypeDto>());
            var platformTypeController = new PlatformTypeController(_platformTypeService.Object, _gameService.Object, Mapper);
            platformTypeController.ModelState.AddModelError("key", "message");

            // Act
            var result = platformTypeController.Create(Mapper.Map<PlatformTypeViewModel>(CreatePlatformTypeDto()));

            // Assert
            Assert.IsType<ViewResult>(result.Result);
        }

        [Fact]
        public void Update_WhenModelValid_ExpectRedirectResultToPlatformTypes()
        {
            // Arrange
            _platformTypeService.Setup(i => i.Update(It.IsAny<PlatformTypeDto>())).Returns(new Result<PlatformTypeDto>());
            var platformTypeController = new PlatformTypeController(_platformTypeService.Object, _gameService.Object, Mapper);

            // Act
            var result = platformTypeController.Update(Mapper.Map<PlatformTypeViewModel>(CreatePlatformTypeDto()));

            // Assert
            var viewResult = Assert.IsType<RedirectResult>(result.Result);
            Assert.Equal("~/platformTypes", viewResult.Url);
        }

        [Fact]
        public void Update_WhenModelNotValidFromUserSide_ExpectViewResult()
        {
            // Arrange
            var resultWithError = new Result<PlatformTypeDto>();
            resultWithError.Errors.Add("Type", "some message");
            _platformTypeService.Setup(i => i.Update(It.IsAny<PlatformTypeDto>())).Returns(resultWithError);
            var platformTypeController = new PlatformTypeController(_platformTypeService.Object, _gameService.Object, Mapper);

            // Act
            var result = platformTypeController.Update(Mapper.Map<PlatformTypeViewModel>(CreatePlatformTypeDto()));

            // Assert
            Assert.IsType<ViewResult>(result.Result);
        }

        [Fact]
        public void Update_WhenModelNotValidFromServerSide_ExpectViewResult()
        {
            // Arrange
            var resultWithError = new Result<PlatformTypeDto>();
            resultWithError.Errors.Add("Type", "some message");
            _platformTypeService.Setup(i => i.Update(It.IsAny<PlatformTypeDto>())).Returns(resultWithError);
            var platformTypeController = new PlatformTypeController(_platformTypeService.Object, _gameService.Object, Mapper);

            // Act
            var result = platformTypeController.Update(Mapper.Map<PlatformTypeViewModel>(CreatePlatformTypeDto()));

            // Assert
            Assert.IsType<ViewResult>(result.Result);
        }

        [Fact]
        public void Remove_Always_ExpectRedirectResultToPlatformTypes()
        {
            // Arrange
            const int id = 1;
            _platformTypeService.Setup(i => i.Delete(id));
            _platformTypeService.Setup(i => i.GetById(id)).Returns(new PlatformTypeDto());
            var platformTypeController = new PlatformTypeController(_platformTypeService.Object, _gameService.Object, Mapper);

            // Act
            var result = platformTypeController.Remove(id);

            // Assert
            var viewResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("~/platformTypes", viewResult.Url);
        }
    }
}

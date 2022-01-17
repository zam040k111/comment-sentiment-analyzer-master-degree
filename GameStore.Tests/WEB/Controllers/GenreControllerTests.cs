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
    public class GenreControllerTests : WebTests
    {
        private readonly Mock<IGenreService> _genreService;
        private readonly Mock<IGameService> _gameService;

        public GenreControllerTests()
        {
            _genreService = new Mock<IGenreService>();
            _gameService = new Mock<IGameService>();
        }

        [Fact]
        public void GetGenres_Always_ExpectViewResultWithGenreViewModelList()
        {
            // Arrange
            _genreService.Setup(i => i.GetAll(false)).Returns(new List<GenreDto>());
            var genreController = new GenreController(_genreService.Object, _gameService.Object, Mapper);

            // Act
            var result = genreController.GetGenres();

            // Assert
            Assert.IsType<ViewResult>(result.Result);
        }

        [Fact]
        public void Create_WhenModelValid_ExpectRedirectResultToGenres()
        {
            // Arrange
            _genreService.Setup(i => i.GetAll(false)).Returns(new List<GenreDto>());
            _genreService.Setup(i => i.Add(It.IsAny<GenreDto>())).Returns(new Result<GenreDto>());
            var genreController = new GenreController(_genreService.Object, _gameService.Object, Mapper);

            // Act
            var result = genreController.Create(Mapper.Map<GenreViewModel>(CreateGenreDto()));

            // Assert
            var viewResult = Assert.IsType<RedirectResult>(result.Result);
            Assert.Equal("~/genres", viewResult.Url);
        }

        [Fact]
        public void Create_WhenModelNotValidFromUserSide_ExpectViewResult()
        {
            // Arrange
            _genreService.Setup(i => i.GetAll(false)).Returns(new List<GenreDto>());
            _genreService.Setup(i => i.Add(It.IsAny<GenreDto>())).Returns(new Result<GenreDto>());
            var genreController = new GenreController(_genreService.Object, _gameService.Object, Mapper);
            genreController.ModelState.AddModelError("key", "message");

            // Act
            var result = genreController.Create(Mapper.Map<GenreViewModel>(CreateGenreDto()));

            // Assert
            Assert.IsType<ViewResult>(result.Result);
        }

        [Fact]
        public void Create_WhenModelNotValidFromServerSide_ExpectViewResult()
        {
            // Arrange
            var resultWithError = new Result<GenreDto>();
            resultWithError.Errors.Add("key", "message");
            _genreService.Setup(i => i.GetAll(false)).Returns(new List<GenreDto>());
            _genreService.Setup(i => i.Add(It.IsAny<GenreDto>())).Returns(resultWithError);
            var genreController = new GenreController(_genreService.Object, _gameService.Object, Mapper);

            // Act
            var result = genreController.Create(Mapper.Map<GenreViewModel>(CreateGenreDto()));

            // Assert
            Assert.IsType<ViewResult>(result.Result);
        }

        [Fact]
        public void Update_WhenModelValid_ExpectRedirectResultToGenres()
        {
            // Arrange
            _genreService.Setup(i => i.GetAll(false)).Returns(new List<GenreDto>());
            _genreService.Setup(i => i.Update(It.IsAny<GenreDto>())).Returns(new Result<GenreDto>());
            var genreController = new GenreController(_genreService.Object, _gameService.Object, Mapper);

            // Act
            var result = genreController.Update(Mapper.Map<GenreViewModel>(CreateGenreDto()));

            // Assert
            var viewResult = Assert.IsType<RedirectResult>(result.Result);
            Assert.Equal("~/genres", viewResult.Url);
        }

        [Fact]
        public void Update_WhenModelNotValidFromUserSide_ExpectViewResult()
        {
            // Arrange
            _genreService.Setup(i => i.GetAll(false)).Returns(new List<GenreDto>());
            _genreService.Setup(i => i.Update(It.IsAny<GenreDto>())).Returns(new Result<GenreDto>());
            var genreController = new GenreController(_genreService.Object, _gameService.Object, Mapper);
            genreController.ModelState.AddModelError("key", "message");

            // Act
            var result = genreController.Update(Mapper.Map<GenreViewModel>(CreateGenreDto()));

            // Assert
            Assert.IsType<ViewResult>(result.Result);
        }

        [Fact]
        public void Update_WhenModelNotValidFromServerSide_ExpectViewResult()
        {
            // Arrange
            var resultWithError = new Result<GenreDto>();
            resultWithError.Errors.Add("key", "message");
            _genreService.Setup(i => i.GetAll(false)).Returns(new List<GenreDto>());
            _genreService.Setup(i => i.Update(It.IsAny<GenreDto>())).Returns(resultWithError);
            var genreController = new GenreController(_genreService.Object, _gameService.Object, Mapper);

            // Act
            var result = genreController.Update(Mapper.Map<GenreViewModel>(CreateGenreDto()));

            // Assert
            Assert.IsType<ViewResult>(result.Result);
        }

        [Fact]
        public void Remove_Always_ExpectRedirectResultToGenres()
        {
            // Arrange
            const int id = 1;
            _genreService.Setup(i => i.Delete(id));
            _genreService.Setup(i => i.GetById(It.IsAny<int>())).Returns(new GenreDto());
            var genreController = new GenreController(_genreService.Object, _gameService.Object, Mapper);

            // Act
            var result = genreController.Remove(id);

            // Assert
            var viewResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("~/genres", viewResult.Url);
        }
    }
}

using System.Collections.Generic;
using GameStore.BLL.DTO;
using GameStore.BLL.Exceptions.ServiceExceptions;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services.Validation;
using GameStore.ML.Interfaces;
using GameStore.Tests.BLL.Services;
using GameStore.WEB.Controllers;
using GameStore.WEB.Interfaces;
using GameStore.WEB.Models;
using GameStore.WEB.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GameStore.Tests.WEB.Controllers
{
    public class GameControllerTests : WebTests
    {
        private readonly Mock<IGameService> _gameService;
        private readonly Mock<ICommentService> _commentService;
        private readonly Mock<IGenreService> _genreService;
        private readonly Mock<IPlatformTypeService> _platformTypeService;
        private readonly Mock<IPublisherService> _publisherService;
        private readonly Mock<IFileService> _fileService;

        public GameControllerTests()
        {
            _gameService = new Mock<IGameService>();
            _commentService = new Mock<ICommentService>();
            _genreService = new Mock<IGenreService>();
            _platformTypeService = new Mock<IPlatformTypeService>();
            _publisherService = new Mock<IPublisherService>();
            _fileService = new Mock<IFileService>();
        }

        [Fact]
        public void GetGame_Always_ExpectViewResult()
        {
            // Arrange
            var gameController = new GameController(
                _gameService.Object,
                _commentService.Object,
                _genreService.Object,
                _platformTypeService.Object,
                _publisherService.Object,
                Mapper,
                _fileService.Object);

            // Act
            var result = gameController.GetGame(null);

            // Assert
            Assert.IsType<ViewResult>(result.Result);
        }

        //[Fact] No idea how to mock Http Request
        //public void Create_WhenModelValid_ExpectRedirectResultToGames()
        //{
        //    // Arrange
        //    _genreService.Setup(i => i.GetAll(false)).Returns(new List<GenreDto>());
        //    _publisherService.Setup(i => i.GetAll(false)).Returns(new List<PublisherDto>());
        //    _platformTypeService.Setup(i => i.GetAll(false)).Returns(new List<PlatformTypeDto>());
        //    _gameService.Setup(i => i.Add(It.IsAny<GameDto>())).Returns(new Result<GameDto>());
        //    var gameController = new GameController(
        //        _gameService.Object,
        //        _commentService.Object,
        //        _genreService.Object,
        //        _platformTypeService.Object,
        //        _publisherService.Object,
        //        Mapper,
        //        _fileService.Object);

        //    // Act
        //    var result = gameController.Create(Mapper.Map<GameViewModel>(
        //        CreateGameDto(
        //        "key",
        //        gameGenresId: new List<int?> { 1 },
        //        gamePlatformTypesId: new List<int?> { 1 })));

        //    // Assert
        //    var viewResult = Assert.IsType<RedirectResult>(result.Result);
        //    Assert.Equal("~/games", viewResult.Url);
        //}

        [Fact]
        public void Create_WhenModelNotValidFromUserSide_ExpectViewResult()
        {
            // Arrange
            _genreService.Setup(i => i.GetAll(false)).Returns(new List<GenreDto>());
            _publisherService.Setup(i => i.GetAll(false)).Returns(new List<PublisherDto>());
            _platformTypeService.Setup(i => i.GetAll(false)).Returns(new List<PlatformTypeDto>());
            _gameService.Setup(i => i.Add(It.IsAny<GameDto>())).Returns(new Result<GameDto>());
            var gameController = new GameController(
                _gameService.Object,
                _commentService.Object,
                _genreService.Object,
                _platformTypeService.Object,
                _publisherService.Object,
                Mapper,
                _fileService.Object);
            gameController.ModelState.AddModelError("key", "message");

            // Act
            var result = gameController.Create(Mapper.Map<GameViewModel>(
                CreateGameDto(
                    "key",
                    gameGenresId: new List<int?> { 1 },
                    gamePlatformTypesId: new List<int?> { 1 })));

            // Assert
            Assert.IsType<ViewResult>(result.Result);
        }

        //[Fact] No idea how to mock Http Request
        //public void Create_WhenModelNotValidFromServerSide_ExpectViewResult()
        //{
        //    // Arrange
        //    var resultWithError = new Result<GameDto>();
        //    resultWithError.Errors.Add("key", "message");
        //    _genreService.Setup(i => i.GetAll(false)).Returns(new List<GenreDto>());
        //    _publisherService.Setup(i => i.GetAll(false)).Returns(new List<PublisherDto>());
        //    _platformTypeService.Setup(i => i.GetAll(false)).Returns(new List<PlatformTypeDto>());
        //    _gameService.Setup(i => i.Add(It.IsAny<GameDto>())).Returns(resultWithError);
        //    var gameController = new GameController(
        //        _gameService.Object,
        //        _commentService.Object,
        //        _genreService.Object,
        //        _platformTypeService.Object,
        //        _publisherService.Object,
        //        Mapper,
        //        _fileService.Object);

        //    // Act
        //    var result = gameController.Create(Mapper.Map<GameViewModel>(
        //        CreateGameDto(
        //            "key",
        //            gameGenresId: new List<int?> { 1 },
        //            gamePlatformTypesId: new List<int?> { 1 })));

        //    // Assert
        //    Assert.IsType<ViewResult>(result.Result);
        //}

        [Fact]
        public void Remove_Always_ExpectRedirectResultToGames()
        {
            // Arrange
            const int id = 1;
            _gameService.Setup(i => i.Delete(id));
            _gameService.Setup(i => i.GetById(It.IsAny<int>())).Returns(new GameDto());
            var gameController = new GameController(
                _gameService.Object,
                _commentService.Object,
                _genreService.Object,
                _platformTypeService.Object,
                _publisherService.Object,
                Mapper,
                _fileService.Object);

            // Act
            var result = gameController.Remove(id);

            // Assert
            var viewResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("~/games", viewResult.Url);
        }

        [Fact]
        public void CreateComment_WhenModelValid_ExpectRedirectResultToComments()
        {
            // Arrange
            const string key = "key";
            _gameService.Setup(i => i.GetByKey(key)).Returns(CreateGameDto());
            var gameController = new GameController(
                _gameService.Object,
                _commentService.Object,
                _genreService.Object,
                _platformTypeService.Object,
                _publisherService.Object,
                Mapper,
                _fileService.Object);

            // Act
            var result = gameController.CreateComment(Mapper.Map<CommentViewModel>(CreateCommentDto()));

            // Assert
            var viewResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("Comments", viewResult.Url);
        }

        [Fact]
        public void CreateComment_WhenModelNotValid_ExpectViewResult()
        {
            // Arrange
            const string key = "key";
            _gameService.Setup(i => i.GetByKey(key)).Returns(CreateGameDto());
            var gameController = new GameController(
                _gameService.Object,
                _commentService.Object,
                _genreService.Object,
                _platformTypeService.Object,
                _publisherService.Object,
                Mapper,
                _fileService.Object);
            gameController.ModelState.AddModelError("key", "message");

            // Act
            var result = gameController.CreateComment(Mapper.Map<CommentViewModel>(CreateCommentDto()));

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void GetComments_Always_ExpectViewResult()
        {
            // Arrange
            const string key = "key";
            _commentService.Setup(i => i.GetAllByGameKey(key, false)).Returns(new List<CommentDto>{CreateCommentDto()});
            var gameController = new GameController(
                _gameService.Object,
                _commentService.Object,
                _genreService.Object,
                _platformTypeService.Object,
                _publisherService.Object,
                Mapper,
                _fileService.Object);

            // Act
            var comments = gameController.Comments(key);

            // Assert
            Assert.IsType<ViewResult>(comments.Result);
        }

        [Fact]
        public void Download_WhenFileExist_ExpectFileStreamResult()
        {
            // Arrange
            const string nameOrGameKey = "testName";
            var file = new FileServiceForTest();
            var fileService = new FileService();
            var gameController = new GameController(
                _gameService.Object,
                _commentService.Object,
                _genreService.Object,
                _platformTypeService.Object,
                _publisherService.Object,
                Mapper,
                fileService);

            // Act
            var result = gameController.Download(nameOrGameKey);

            // Assert
            Assert.IsType<FileStreamResult>(result);

            file.RemoveCreatedDirectory();
        }

        [Fact]
        public void Download_WhenFileNotExist_ExpectNotFoundException()
        {
            // Arrange
            const string nameOrGameKey = "testName";
            var fileService = new FileService();
            var gameController = new GameController(
                _gameService.Object,
                _commentService.Object,
                _genreService.Object,
                _platformTypeService.Object,
                _publisherService.Object,
                Mapper,
                fileService);

            // Act

            // Assert
            Assert.Throws<NotFoundException>(() => gameController.Download(nameOrGameKey));
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.Exceptions.ServiceExceptions;
using GameStore.DAL.Entities;
using GameStore.Tests.DAL;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GameStore.Tests.BLL.Services
{
    public class GameServiceTests : BllTests
    {
        [Fact]
        public void Add_WhenGameKeyAlreadyExist_ExpectErrorWithKey()
        {
            // Arrange
            const string gameKey = "key";
            using var context = new ContextTest(Options);
            context.Games.Add(CreateGame(gameKey));
            context.SaveChanges();
            var gameService = GetGameService(context);

            // Act
            var result = gameService.Add(CreateGameDto(gameKey));

            // Assert
            Assert.True(!result.IsValid);
            Assert.True(result.Errors.ContainsKey("Key"));
        }

        [Fact]
        public void Add_WhenGenreNull_ExpectModelResultWithoutErrors()
        {
            // Arrange
            const string gameKey = "key";
            using var context = new ContextTest(Options);
            var platform = context.PlatformTypes.Add(CreatePlatformType()).Entity;
            var publisher = context.Publishers.Add(CreatePublisher()).Entity;
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameDto = CreateGameDto(gameKey, publisher: publisher.Id, gamePlatformTypesId: new List<int?> {platform.Id});

            // Act
            var result = gameService.Add(gameDto);
            var newEntity = context.Games.Find(result.Value.Id);

            // Assert

            Assert.NotEmpty(newEntity.GamePlatformTypes);
            Assert.True(result.IsValid);
            Assert.Equal(gameDto.GamePlatformTypesId[0],
                newEntity.GamePlatformTypes.FirstOrDefault()?.PlatformTypeId);
        }

        [Fact]
        public void Add_WhenPlatformTypeInvalid_ExpectErrorWithKeyPlatformTypes()
        {
            // Arrange
            const string gameKey = "key";
            using var context = new ContextTest(Options);
            var genre = context.Genres.Add(CreateGenre()).Entity;
            var publisher = context.Publishers.Add(CreatePublisher()).Entity;
            context.SaveChanges();
            var gameService = GetGameService(context);

            // Act
            var result = gameService.Add(CreateGameDto(
                gameKey,
                publisher: publisher.Id,
                gameGenresId: new List<int?> {genre.Id},
                gamePlatformTypesId: new List<int?>{1}));

            // Assert
            Assert.True(result.Errors.ContainsKey("GamePlatformTypesId"));
        }

        [Fact]
        public void Add_WhenEntityValid_ExpectModelIsValid()
        {
            // Arrange
            const string key = "key";
            using var context = new ContextTest(Options);
            var platform = context.PlatformTypes.Add(CreatePlatformType()).Entity;
            var genre = context.Genres.Add(CreateGenre()).Entity;
            var publisher = context.Publishers.Add(CreatePublisher()).Entity;
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameDto = CreateGameDto(
                key,
                publisher: publisher.Id,
                gameGenresId: new List<int?> {genre.Id},
                gamePlatformTypesId: new List<int?> {platform.Id});

            // Act
            var result = gameService.Add(gameDto);
            var newEntity = context.Games
                .Include(gen => gen.GameGenres)
                .Include(pl => pl.GamePlatformTypes)
                .FirstOrDefault(i => i.Key.Equals(key));

            // Assert
            Assert.NotNull(newEntity);
            Assert.NotEmpty(newEntity.GameGenres);
            Assert.NotEmpty(newEntity.GamePlatformTypes);
            Assert.True(result.IsValid);
            Assert.Equal(gameDto.GameGenresId[0],
                newEntity.GameGenres.FirstOrDefault()?.GenreId);
            Assert.Equal(gameDto.GamePlatformTypesId[0],
                newEntity.GamePlatformTypes.FirstOrDefault()?.PlatformTypeId);
        }

        [Fact]
        public void GetById_WhenGameExist_ExpectGameEntity()
        {
            // Arrange
            const string gameKey = "key";
            using var context = new ContextTest(Options);
            var game = context.Games.Add(CreateGame(gameKey)).Entity;
            context.SaveChanges();
            var gameService = GetGameService(context);

            // Act
            var gameEntity = gameService.GetById(game.Id);

            // Assert
            Assert.Equal(game.Key, gameEntity.Key);
            Assert.Equal(game.Id, gameEntity.Id);
        }

        [Fact]
        public void GetById_WhenGameNotExist_ExpectNotFoundException()
        {
            // Arrange
            const int id = 2;
            using var context = new ContextTest(Options);
            var gameService = GetGameService(context);

            // Act

            // Assert
            Assert.Throws<NotFoundException>(() => gameService.GetById(id));
        }

        [Fact]
        public void GetAll_WhenGamesExist_ExpectNotEmptyList()
        {
            // Arrange
            using var context = new ContextTest(Options);
            context.Games.Add(CreateGame());
            context.SaveChanges();
            var gameService = GetGameService(context);

            // Act
            var result = gameService.GetAll();

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetAll_WhenGamesNotExist_ExpectEmptyList()
        {
            // Arrange
            using var context = new ContextTest(Options);
            var gameService = GetGameService(context);

            // Act
            var result = gameService.GetAll();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetAll_WhenGamesIsDeleted_ExpectEmptyList()
        {
            // Arrange
            using var context = new ContextTest(Options);
            var gameService = GetGameService(context);
            var game = context.Games.Add(CreateGame("key", isDeleted: true)).Entity;

            // Act
            var result = gameService.GetAll();

            // Assert
            Assert.Empty(result);
            Assert.True(context.Games.Find(game.Id).IsDeleted);
        }

        [Fact]
        public void GetByGenre_WhenGenreExists_ExpectNotEmptyList()
        {
            // Arrange
            const string genreName = "genre";
            const string gameKey = "key";
            using var context = new ContextTest(Options);
            var genre = context.Genres.Add(CreateGenre(genreName)).Entity;
            context.Games.Add(CreateGame(
                gameKey,
                gameGenres:new List<GameGenre> { new GameGenre { GenreId = genre.Id } }));
            context.SaveChanges();
            var gameService = GetGameService(context);

            // Act
            var result = gameService.GetByGenre(genre.Id).FirstOrDefault();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.GameGenresId);
            Assert.Equal(genre.Id, result.GameGenresId[0]);
        }

        [Fact]
        public void GetByGenre_WhenGenreNotExist_ExpectEmptyList()
        {
            //Arrange
            const string gameKey = "key";
            const int genreId = 1;
            using var context = new ContextTest(Options);
            context.Games.Add(CreateGame(gameKey));
            context.SaveChanges();
            var gameService = GetGameService(context);

            //Act
            var result = gameService.GetByGenre(genreId);

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetByKey_WhenKeyNotExist_ExpectNotFoundException()
        {
            // Arrange
            const string gameKey = "key";
            using var context = new ContextTest(Options);
            var gameService = GetGameService(context);

            // Act

            // Assert
            Assert.Throws<NotFoundException>(() => gameService.GetByKey(gameKey));
        }

        [Fact]
        public void GetByKey_WhenKeyExist_ExpectGameEntity()
        {
            // Arrange
            const string gameKey = "key";
            using var context = new ContextTest(Options);
            var game = context.Games.Add(CreateGame(gameKey)).Entity;
            context.SaveChanges();
            using var context2 = new ContextTest(Options);
            var gameService = GetGameService(context2);

            // Act
            var result = gameService.GetByKey(game.Key);

            // Assert
            Assert.Equal(game.Key, result.Key);
            Assert.Equal(game.Id, result.Id);
        }

        [Fact]
        public void GetByPlatformType_WhenPlatformTypeExist_ExpectNotEmptyList()
        {
            // Arrange
            const string gameKey = "key";
            using var context = new ContextTest(Options);
            var platform = context.PlatformTypes.Add(CreatePlatformType()).Entity;
            context.Games.Add(CreateGame(
                gameKey,
                gamePlatformTypes: new List<GamePlatformType> { new GamePlatformType { PlatformTypeId = platform.Id } }));
            context.SaveChanges();
            var gameService = GetGameService(context);

            // Act
            var result = gameService.GetByPlatformType(platform.Id);

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetByPlatformType_WhenPlatformTypeNotExist_ExpectEmptyList()
        {
            // Arrange
            const string gameKey = "key";
            const int platformId = 1;
            using var context = new ContextTest(Options);
            context.Games.Add(CreateGame(gameKey));
            context.SaveChanges();
            var gameService = GetGameService(context);

            // Act
            var result = gameService.GetByPlatformType(platformId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetByPublisher_WhenPublisherExist_ExpectNotEmptyList()
        {
            // Arrange
            const string gameKey = "key";
            using var context = new ContextTest(Options);
            var publisher = context.Publishers.Add(CreatePublisher()).Entity;
            context.Games.Add(CreateGame(
                gameKey,
                publisher: publisher));
            context.SaveChanges();
            var gameService = GetGameService(context);

            // Act
            var result = gameService.GetByPublisher(publisher.Id);

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetByPublisher_WhenPublisherNotExist_ExpectEmptyList()
        {
            // Arrange
            const string gameKey = "key";
            const int publisherId = 1;
            using var context = new ContextTest(Options);
            context.Games.Add(CreateGame(gameKey));
            context.SaveChanges();
            var gameService = GetGameService(context);

            // Act
            var result = gameService.GetByPublisher(publisherId);

            // Assert
            Assert.Empty(result);
        }
    }
}

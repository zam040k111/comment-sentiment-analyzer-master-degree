using GameStore.DAL.Entities;
using System.Collections.Generic;
using GameStore.BLL.Exceptions.ServiceExceptions;
using GameStore.Tests.DAL;
using Xunit;

namespace GameStore.Tests.BLL.Services
{
    public class GenreServiceTests : BllTests
    {
        [Fact]
        public void Add_WhenGenreNameInSameCategoryAlreadyExist_ExpectErrorWithNameKey()
        {
            // Arrange
            const string genreName = "genre";
            using var context = new ContextTest(Options);
            var genre = context.Genres.Add(CreateGenre(genreName)).Entity;
            context.SaveChanges();
            var genreService = GetGenreService(context);

            // Act
            var result = genreService.Add(CreateGenreDto(genre.Name));

            // Assert
            Assert.True(!result.IsValid);
            Assert.True(result.Errors.ContainsKey("Name"));
        }

        [Fact]
        public void Add_WhenGenreValid_ExpectModelStateIsValid()
        {
            // Arrange
            const string genreName = "genre";
            using var context = new ContextTest(Options);
            var genreService = GetGenreService(context);

            // Act
            var result = genreService.Add(CreateGenreDto(genreName));
            var expectedEntity = context.Genres.Find(result.Value.Id);

            // Assert
            Assert.True(result.IsValid);
            Assert.NotNull(expectedEntity);
        }

        [Fact]
        public void Update_WhenNameInSameCategoryAlreadyExist_ExpectErrorWithNameKey()
        {
            // Arrange
            const string genreName = "genre";
            const string genreName2 = "genre2";
            using var context = new ContextTest(Options);
            context.Genres.Add(CreateGenre(genreName));
            var genre = context.Genres.Add(CreateGenre(genreName2)).Entity;
            context.SaveChanges();
            var genreService = GetGenreService(context);

            // Act
            genre.Name = genreName;
            var result = genreService.Update(CreateGenreDto(genre.Name, id: genre.Id));

            // Assert
            Assert.True(!result.IsValid);
            Assert.True(result.Errors.ContainsKey("Name"));
        }

        [Fact]
        public void Update_WhenEntityValid_ExpectModelStateIsValid()
        {
            // Arrange
            const string genreName = "genre";
            const string updatedGenreName = "new genre";
            using var context = new ContextTest(Options);
            var genre = context.Genres.Add(CreateGenre(genreName)).Entity;
            context.SaveChanges();
            using var contextForUpdate = new ContextTest(Options);
            var genreService = GetGenreService(contextForUpdate);

            // Act
            var result = genreService.Update(CreateGenreDto(updatedGenreName, id: genre.Id));

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(updatedGenreName, contextForUpdate.Genres.Find(result.Value.Id).Name);
        }

        [Fact]
        public void Delete_WhenGenreExist_ExpectEntityIsDeleted()
        {
            // Arrange
            const string genreName = "genre";
            using var context = new ContextTest(Options);
            var genre = context.Genres.Add(CreateGenre(genreName)).Entity;
            context.SaveChanges();
            using var contextForDelete = new ContextTest(Options);
            var genreService = GetGenreService(contextForDelete);

            // Act
            genreService.Delete(genre.Id);

            // Assert
            Assert.True(contextForDelete.Genres.Find(genre.Id).IsDeleted);
        }

        [Fact]
        public void GetByName_WhenGenreNotExist_ExpectNotFoundException()
        {
            // Arrange
            const string genreName = "genre";
            using var context = new ContextTest(Options);
            var genreService = GetGenreService(context);

            // Act

            // Assert
            Assert.Throws<NotFoundException>(() => genreService.GetByName(genreName));
        }

        [Fact]
        public void GetByName_WhenGenreExist_ExpectEntity()
        {
            // Arrange
            const string genreName = "genre";
            using var context = new ContextTest(Options);
            var genre = context.Genres.Add(CreateGenre(genreName)).Entity;
            context.SaveChanges();
            var genreService = GetGenreService(context);

            // Act
            var result = genreService.GetByName(genreName);

            // Assert
            Assert.Equal(genre.Id, result.Id);
        }

        [Fact]
        public void GetByGame_WhenGameNotExist_ExpectEmptyList()
        {
            // Arrange
            const int notExistGameId = 1;
            using var context = new ContextTest(Options);
            var genreService = GetGenreService(context);

            // Act
            var result = genreService.GetByGame(notExistGameId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetByGame_WhenGameExist_ExpectNotEmptyList()
        {
            // Arrange
            const string gameKey = "key";
            const string genreName = "genre";
            using var context = new ContextTest(Options);
            var genre = context.Genres.Add(CreateGenre(genreName)).Entity;
            var game = context.Games.Add(CreateGame(
                gameKey, 
                gameGenres: new List<GameGenre> { new GameGenre { GenreId = genre.Id } })).Entity;
            context.SaveChanges();
            var genreService = GetGenreService(context);

            // Act
            var result = genreService.GetByGame(game.Id);

            // Assert
            Assert.NotEmpty(result);
        }
    }
}

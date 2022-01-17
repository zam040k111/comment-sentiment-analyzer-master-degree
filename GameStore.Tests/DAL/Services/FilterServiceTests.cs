using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.DTO;
using GameStore.DAL.Entities;
using GameStore.DAL.Services.Filters;
using GameStore.Tests.BLL;
using Xunit;

namespace GameStore.Tests.DAL.Services
{
    public class FilterServiceTests : BllTests
    {
        [Fact]
        public void Filter_WhenNoFilterWasApply_ExpectNotEmptyList()
        {
            // Arrange
            using var context = new ContextTest(Options);
            context.Add(CreateGame());
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameFilter = new GameFilterDto();

            // Act
            gameFilter = gameService.ApplyFilter(gameFilter);

            // Assert
            Assert.NotEmpty(gameFilter.GamesPerPage);
        }

        [Fact]
        public void GenreFilter_WhenFilterWasApplyAndSuchEntityNotExist_ExpectEmptyList()
        {
            // Arrange
            using var context = new ContextTest(Options);
            context.Add(CreateGame());
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameFilter = new GameFilterDto { Genres = new List<int?> { 2 } };

            // Act
            gameFilter = gameService.ApplyFilter(gameFilter);

            // Assert
            Assert.Empty(gameFilter.GamesPerPage);
        }

        [Fact]
        public void GenreFilter_WhenFilterWasApplyAndSuchEntityExist_ExpectNotEmptyList()
        {
            // Arrange
            using var context = new ContextTest(Options);
            context.Add(CreateGame(gameGenres: new List<GameGenre> { new GameGenre { GenreId = 1, GameId = 1 } }));
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameFilter = new GameFilterDto { Genres = new List<int?> { 1 } };

            // Act
            gameFilter = gameService.ApplyFilter(gameFilter);

            // Assert
            Assert.NotEmpty(gameFilter.GamesPerPage);
        }

        [Fact]
        public void PlatformsFilter_WhenFilterWasApplyAndSuchEntityExist_ExpectNotEmptyList()
        {
            // Arrange
            using var context = new ContextTest(Options);
            context.Add(CreateGame(gamePlatformTypes: new List<GamePlatformType> { new GamePlatformType() { PlatformTypeId = 1, GameId = 1 } }));
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameFilter = new GameFilterDto { Platforms = new List<int?> { 1 } };

            // Act
            gameFilter = gameService.ApplyFilter(gameFilter);

            // Assert
            Assert.NotEmpty(gameFilter.GamesPerPage);
        }

        [Fact]
        public void PlatformsFilter_WhenFilterWasApplyAndSuchEntityNotExist_ExpectEmptyList()
        {
            // Arrange
            using var context = new ContextTest(Options);
            context.Add(CreateGame());
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameFilter = new GameFilterDto { Platforms = new List<int?> { 2 } };

            // Act
            gameFilter = gameService.ApplyFilter(gameFilter);

            // Assert
            Assert.Empty(gameFilter.GamesPerPage);
        }

        [Fact]
        public void NameFilter_WhenFilterWasApplyAndSuchEntityNotExist_ExpectEmptyList()
        {
            // Arrange
            const string name = "name";
            const string textForFiltering = "text";
            using var context = new ContextTest(Options);
            context.Add(CreateGame(name: name));
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameFilter = new GameFilterDto { Name = textForFiltering };

            // Act
            gameFilter = gameService.ApplyFilter(gameFilter);

            // Assert
            Assert.Empty(gameFilter.GamesPerPage);
        }

        [Fact]
        public void NameFilter_WhenFilterWasApplyAndSuchEntityExist_ExpectNotEmptyList()
        {
            // Arrange
            const string name = "some name";
            const string textForFiltering = "ame";
            using var context = new ContextTest(Options);
            context.Add(CreateGame(name: name));
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameFilter = new GameFilterDto { Name = textForFiltering };

            // Act
            gameFilter = gameService.ApplyFilter(gameFilter);

            // Assert
            Assert.NotEmpty(gameFilter.GamesPerPage);
        }

        [Fact]
        public void PriceFromFilter_WhenFilterWasApplyAndSuchEntityExist_ExpectNotEmptyList()
        {
            // Arrange
            const decimal price = 10;
            const decimal priceForFiltering = 8;
            using var context = new ContextTest(Options);
            context.Add(CreateGame(price: price));
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameFilter = new GameFilterDto { PriceFrom = priceForFiltering };

            // Act
            gameFilter = gameService.ApplyFilter(gameFilter);

            // Assert
            Assert.NotEmpty(gameFilter.GamesPerPage);
        }

        [Fact]
        public void PriceToFilter_WhenFilterWasApplyAndSuchEntityExist_ExpectNotEmptyList()
        {
            // Arrange
            const decimal price = 10;
            const decimal priceForFiltering = 12;
            using var context = new ContextTest(Options);
            context.Add(CreateGame(price: price));
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameFilter = new GameFilterDto { PriceTo = priceForFiltering };

            // Act
            gameFilter = gameService.ApplyFilter(gameFilter);

            // Assert
            Assert.NotEmpty(gameFilter.GamesPerPage);
        }

        [Fact]
        public void PriceFromToFilter_WhenFilterWasApplyAndSuchEntityExist_ExpectNotEmptyList()
        {
            // Arrange
            const decimal price = 10;
            const decimal priceForFilteringFrom = 8;
            const decimal priceForFilteringTo = 12;
            using var context = new ContextTest(Options);
            context.Add(CreateGame(price: price));
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameFilter = new GameFilterDto { PriceFrom = priceForFilteringFrom, PriceTo = priceForFilteringTo };

            // Act
            gameFilter = gameService.ApplyFilter(gameFilter);

            // Assert
            Assert.NotEmpty(gameFilter.GamesPerPage);
        }

        [Fact]
        public void PublishedFilter_WhenFilterWasApplyAndSuchEntityNotExist_ExpectEmptyList()
        {
            // Arrange
            var span = TimeSpan.FromDays(7);
            var publishedDate = DateTime.MinValue;
            publishedDate = publishedDate.AddYears(2000);
            using var context = new ContextTest(Options);
            context.Add(CreateGame(published: publishedDate));
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameFilter = new GameFilterDto { Published = span };

            // Act
            gameFilter = gameService.ApplyFilter(gameFilter);

            // Assert
            Assert.Empty(gameFilter.GamesPerPage);
        }

        [Fact]
        public void PublishedFilter_WhenFilterWasApplyAndSuchEntityExist_ExpectNotEmptyList()
        {
            // Arrange
            var span = TimeSpan.FromDays(365);
            var publishedDate = DateTime.MinValue;
            publishedDate = publishedDate.AddYears(2020);
            using var context = new ContextTest(Options);
            context.Add(CreateGame(published: publishedDate));
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameFilter = new GameFilterDto { Published = span };

            // Act
            gameFilter = gameService.ApplyFilter(gameFilter);

            // Assert
            Assert.NotEmpty(gameFilter.GamesPerPage);
        }

        [Fact]
        public void SortByMostPopularFilter_WhenFilterWasApply_ExpectSortedList()
        {
            // Arrange
            using var context = new ContextTest(Options);
            context.Add(CreateGame(viewed: 1));
            context.Add(CreateGame(viewed: 2));
            var mostPopular = context.Add(CreateGame(viewed: 3)).Entity;
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameFilter = new GameFilterDto { SortBy = SortBy.MostPopular };

            // Act
            gameFilter = gameService.ApplyFilter(gameFilter);
            var result = gameFilter.GamesPerPage.FirstOrDefault();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mostPopular.Viewed, result.Viewed);
        }

        [Fact]
        public void SortByMostCommentedFilter_WhenFilterWasApply_ExpectSortedList()
        {
            // Arrange
            using var context = new ContextTest(Options);
            context.Add(CreateGame());
            context.Add(CreateGame());
            var mostCommented = context.Add(CreateGame()).Entity;
            context.Add(CreateComment(gameId: mostCommented.Id));
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameFilter = new GameFilterDto { SortBy = SortBy.MostCommented };

            // Act
            gameFilter = gameService.ApplyFilter(gameFilter);
            var result = gameFilter.GamesPerPage.FirstOrDefault();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mostCommented.Id, result.Id);
        }

        [Fact]
        public void SortByPriceAscFilter_WhenFilterWasApply_ExpectSortedList()
        {
            // Arrange
            using var context = new ContextTest(Options);
            context.Add(CreateGame(price: 20));
            var priceAsc = context.Add(CreateGame(price: 10)).Entity;
            context.Add(CreateGame(price: 15));
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameFilter = new GameFilterDto { SortBy = SortBy.PriceASC };

            // Act
            gameFilter = gameService.ApplyFilter(gameFilter);
            var result = gameFilter.GamesPerPage.FirstOrDefault();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(priceAsc.Id, result.Id);
        }

        [Fact]
        public void SortByPriceDescFilter_WhenFilterWasApply_ExpectSortedList()
        {
            // Arrange
            using var context = new ContextTest(Options);
            context.Add(CreateGame(price: 20));
            var priceDesc = context.Add(CreateGame(price: 30)).Entity;
            context.Add(CreateGame(price: 15));
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameFilter = new GameFilterDto { SortBy = SortBy.PriceDESC };

            // Act
            gameFilter = gameService.ApplyFilter(gameFilter);
            var result = gameFilter.GamesPerPage.FirstOrDefault();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(priceDesc.Id, result.Id);
        }

        [Fact]
        public void SortByNewFilter_WhenFilterWasApply_ExpectSortedList()
        {
            // Arrange
            using var context = new ContextTest(Options);
            var date = DateTime.MinValue;
            date = date.AddYears(2018);
            var date2 = DateTime.MinValue;
            date2 = date2.AddYears(2019);
            var date3 = DateTime.MinValue;
            date3 = date3.AddYears(2020);
            context.Add(CreateGame(published: date2));
            var sortByNew = context.Add(CreateGame(published: date3)).Entity;
            context.Add(CreateGame(published: date));
            context.SaveChanges();
            var gameService = GetGameService(context);
            var gameFilter = new GameFilterDto { SortBy = SortBy.Published };

            // Act
            gameFilter = gameService.ApplyFilter(gameFilter);
            var result = gameFilter.GamesPerPage.FirstOrDefault();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(sortByNew.Id, result.Id);
        }
    }
}

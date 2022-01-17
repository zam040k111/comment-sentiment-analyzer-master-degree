using System;
using System.Collections.Generic;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Mapper;
using GameStore.BLL.Services;
using GameStore.DAL;
using GameStore.DAL.Entities;
using GameStore.ML.Interfaces;
using GameStore.ML.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace GameStore.Tests.BLL
{
    public class BllTests
    {
        protected DbContextOptions<GameStoreContext> Options { get; }

        private readonly IMapper _mapper;
        private readonly Mock<ISentimentService> _sentimentService;

        protected BllTests()
        {
            var serviceProvider = new ServiceCollection()
           .AddEntityFrameworkInMemoryDatabase()
           .BuildServiceProvider();

            Options = new DbContextOptionsBuilder<GameStoreContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            var mapper = new MapperConfiguration(config => config.AddProfile(new MapperConfigDto()));
            _mapper = mapper.CreateMapper();
            _sentimentService = new Mock<ISentimentService>();
            _sentimentService.Setup(i => i.PredictSentiment(It.IsAny<string>())).Returns(new float[] { 0.5F });
        }

        protected CommentService GetCommentService(GameStoreContext context) =>
            new CommentService(new UnitOfWork(context, null, null), _mapper, _sentimentService.Object);

        protected GameService GetGameService(GameStoreContext context) =>
            new GameService(new UnitOfWork(context, null, null), _mapper);

        protected GenreService GetGenreService(GameStoreContext context) =>
            new GenreService(new UnitOfWork(context, null, null), _mapper);

        protected PlatformTypeService GetPlatformTypeService(GameStoreContext context) =>
            new PlatformTypeService(new UnitOfWork(context, null, null), _mapper);

        protected PublisherService GetPublisherService(GameStoreContext context) =>
            new PublisherService(new UnitOfWork(context, null, null), _mapper);

        protected OrderService GetOrderService(GameStoreContext context) =>
            new OrderService(new UnitOfWork(context, null, null), _mapper);

        protected Order CreateOrder(string customerId = "id")
        {
            return new Order
            {
                CustomerId = customerId
            };
        }

        protected OrderDto CreateOrderDto(string customerId = "id", List<OrderDetailDto> orderDetails = null)
        {
            return new OrderDto
            {
                CustomerId = customerId,
                OrderDetails = orderDetails
            };
        }

        protected Publisher CreatePublisher(
            string companyName = "name",
            string desc = "desc",
            string homePage = "",
            int id = 1)
        {
            return new Publisher
            {
                Id = id,
                CompanyName = companyName,
                Description = desc,
                HomePage = homePage
            };
        }

        protected PublisherDto CreatePublisherDto(
            string companyName = "name",
            string desc = "desc",
            string homePage = "",
            int id = 1)
        {
            return new PublisherDto
            {
                Id = id,
                CompanyName = companyName,
                Description = desc,
                HomePage = homePage
            };
        }

        protected Comment CreateComment(string name = "", string body = "", int gameId = 0, int id = 1)
        {
            return new Comment
            {
                Id = id,
                Body = body,
                GameId = gameId,
                Name = name
            };
        }
        protected CommentDto CreateCommentDto(string name = "", string body = "", int id = 1)
        {
            return new CommentDto
            {
                Id = id,
                Body = body,
                Name = name
            };
        }

        protected Game CreateGame(
            string key = "",
            string name = "",
            List<GameGenre> gameGenres = null,
            List<GamePlatformType> gamePlatformTypes = null,
            Publisher publisher = null,
            decimal price = 0,
            DateTime published = new DateTime(),
            int viewed = 0,
            bool isDeleted = false)
        {
            return new Game
            {
                Key = key,
                Name = name,
                GameGenres = gameGenres,
                GamePlatformTypes = gamePlatformTypes,
                Publisher = publisher,
                Price = price,
                Published = published,
                Viewed = viewed,
                IsDeleted = isDeleted
            };
        }

        protected PlatformType CreatePlatformType(
            string type = "Mobile",
            List<GamePlatformType> gamePlatformTypes = null)
        {
            return new PlatformType
            {
                Type = type,
                GamePlatformTypes = gamePlatformTypes,
            };
        }

        protected PlatformTypeDto CreatePlatformTypeDto(
            string type = "Mobile",
            List<int?> gamePlatformTypes = null,
            int id = 1)
        {
            return new PlatformTypeDto
            {
                Id = id,
                Type = type,
                GamePlatformTypesId = gamePlatformTypes,
            };
        }

        protected Genre CreateGenre(
            string name = "",
            List<GameGenre> gameGenres = null,
            int? parentGenreId = null)
        {
            return new Genre
            {
                Name = name,
                GameGenres = gameGenres,
                ParentGenreId = parentGenreId
            };
        }

        protected GenreDto CreateGenreDto(
            string name = "",
            List<int?> gameGenres = null,
            int id = 0)
        {
            return new GenreDto
            {
                Id = id,
                Name = name,
                GameGenresId = gameGenres
            };
        }

        protected GameDto CreateGameDto(
            string key = "",
            string name = "",
            int? publisher = 1,
            List<int?> gameGenresId = null,
            List<int?> gamePlatformTypesId = null,
            short unitInStock = 1)
        {
            return new GameDto
            {
                Key = key,
                Name = name,
                GameGenresId = gameGenresId,
                GamePlatformTypesId = gamePlatformTypesId,
                UnitsInStock = unitInStock,
                PublisherId = publisher
            };
        }
    }
}

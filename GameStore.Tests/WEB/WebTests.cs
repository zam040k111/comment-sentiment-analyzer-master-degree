using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.WEB.Mapper;
using System.Collections.Generic;
using GameStore.DAL.Entities;

namespace GameStore.Tests.WEB
{
    public class WebTests
    {
        protected readonly IMapper Mapper;

        public WebTests()
        {
            var mapper = new MapperConfiguration(config => 
                config.AddProfiles(new List<Profile>{new MapperConfigWeb(), new MapperConfigPdfData()}));
            Mapper = mapper.CreateMapper();
        }

        protected GenreDto CreateGenreDto(
            string name = "",
            List<int?> gameGenres = null,
            int id = 1)
        {
            return new GenreDto
            {
                Id = id,
                Name = name,
                GameGenresId = gameGenres
            };
        }

        protected CommentDto CreateCommentDto(string name = "name", string body = "")
        {
            return new CommentDto
            {
                Name = name,
                Body = body
            };
        }

        protected PlatformTypeDto CreatePlatformTypeDto(string type = "name", int id = 1)
        {
            return new PlatformTypeDto
            {
                Id = id,
                Type = type
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

        protected GameDto CreateGameDto(
            string key = "",
            string name = "",
            List<int?> gameGenresId = null,
            List<int?> gamePlatformTypesId = null,
            int id = 0)
        {
            return new GameDto
            {
                Id = id,
                Key = key,
                Name = name,
                GameGenresId = gameGenresId,
                GamePlatformTypesId = gamePlatformTypesId
            };
        }

        protected Game CreateGame(
            string key = "",
            string name = "",
            List<GameGenre> gameGenres = null,
            List<GamePlatformType> gamePlatformTypes = null,
            bool isDeleted = false)
        {
            return new Game
            {
                Key = key,
                Name = name,
                GameGenres = gameGenres,
                GamePlatformTypes = gamePlatformTypes,
                IsDeleted = isDeleted
            };
        }
    }
}

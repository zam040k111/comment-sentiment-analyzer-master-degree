using System.Collections.Generic;
using GameStore.BLL.Exceptions.ServiceExceptions;
using GameStore.DAL.Entities;
using GameStore.Tests.DAL;
using Xunit;

namespace GameStore.Tests.BLL.Services
{
    public class PlatformTypeServiceTests : BllTests
    {
        [Fact]
        public void Add_WhenPlatformTypeValid_ExpectModelIsValid()
        {
            // Arrange
            const string type = "Mobile";
            using var context = new ContextTest(Options);
            var platformTypeService = GetPlatformTypeService(context);

            // Act
            var result = platformTypeService.Add(CreatePlatformTypeDto());
            var expectedEntity = context.PlatformTypes.Find(result.Value.Id);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(expectedEntity.Type, type);
        }

        [Fact]
        public void Add_WhenPlatformTypeAlreadyExist_ExpectModelResultWithTypeKey()
        {
            // Arrange
            const int id = 2;
            using var context = new ContextTest(Options);
            context.PlatformTypes.Add(CreatePlatformType());
            context.SaveChanges();
            using var contextForAdd = new ContextTest(Options);
            var platformTypeService = GetPlatformTypeService(contextForAdd);

            // Act
            var result = platformTypeService.Add(CreatePlatformTypeDto(id: id));

            // Assert
            Assert.True(!result.IsValid);
            Assert.True(result.Errors.ContainsKey("Type"));
        }

        [Fact]
        public void Update_WhenEntityValid_ExpectModelIsValid()
        {
            // Arrange
            const string type = "Browser";
            using var context = new ContextTest(Options);
            var platformType = context.PlatformTypes.Add(CreatePlatformType()).Entity;
            context.SaveChanges();
            using var contextForUpdate = new ContextTest(Options);
            var platformTypeService = GetPlatformTypeService(contextForUpdate);

            // Act
            var result = platformTypeService.Update(CreatePlatformTypeDto(type, id: platformType.Id));

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal(type, contextForUpdate.PlatformTypes.Find(result.Value.Id).Type);
        }

        [Fact]
        public void Update_WhenPlatformTypeAlreadyExist_ExpectModelResultWithTypeKey()
        {
            // Arrange
            const int id = 2;
            using var context = new ContextTest(Options);
            context.PlatformTypes.Add(CreatePlatformType());
            context.SaveChanges();
            using var contextForAdd = new ContextTest(Options);
            var platformTypeService = GetPlatformTypeService(contextForAdd);

            // Act
            var result = platformTypeService.Update(CreatePlatformTypeDto(id: id));

            // Assert
            Assert.True(!result.IsValid);
            Assert.True(result.Errors.ContainsKey("Type"));
        }

        [Fact]
        public void Delete_WhenPlatformTypeExist_ExpectEntityIsDeleted()
        {
            // Arrange
            using var context = new ContextTest(Options);
            var platformType = context.PlatformTypes.Add(CreatePlatformType()).Entity;
            context.SaveChanges();
            using var contextForDelete = new ContextTest(Options);
            var platformTypeService = GetPlatformTypeService(contextForDelete);

            // Act
            platformTypeService.Delete(platformType.Id);

            // Assert
            Assert.True(contextForDelete.PlatformTypes.Find(platformType.Id).IsDeleted);
        }

        [Fact]
        public void Delete_WhenPlatformTypeNotExist_ExpectNotFoundException()
        {
            // Arrange
            using var context = new ContextTest(Options);
            var platformTypeService = GetPlatformTypeService(context);

            // Act

            // Assert
            Assert.Throws<NotFoundException>(() => platformTypeService.Delete(1));
        }

        [Fact]
        public void GetById_WhenPlatformTypeExist_ExpectEntity()
        {
            // Arrange
            using var context = new ContextTest(Options);
            var platformType = context.PlatformTypes.Add(CreatePlatformType()).Entity;
            context.SaveChanges();
            using var contextForDelete = new ContextTest(Options);
            var platformTypeService = GetPlatformTypeService(contextForDelete);

            // Act
            var result = platformTypeService.GetById(platformType.Id);

            // Assert
            Assert.Equal(platformType.Type, result.Type);
        }

        [Fact]
        public void GetById_WhenPlatformTypeNotExist_ExpectNotFoundException()
        {
            // Arrange
            using var context = new ContextTest(Options);
            var platformTypeService = GetPlatformTypeService(context);

            // Act

            // Assert
            Assert.Throws<NotFoundException>(() => platformTypeService.GetById(1));
        }

        [Fact]
        public void GetByGame_WhenGameExist_ExpectNotEmptyList()
        {
            // Arrange
            using var context = new ContextTest(Options);
            var platform = context.PlatformTypes.Add(CreatePlatformType()).Entity;
            var game = context.Games.Add(CreateGame(
                gamePlatformTypes: new List<GamePlatformType> 
                    { new GamePlatformType { PlatformTypeId = platform.Id} }
                )).Entity;
            context.SaveChanges();
            var platformTypeService = GetPlatformTypeService(context);

            // Act
            var result = platformTypeService.GetByGame(game.Id);

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetByGame_WhenGameNotExist_ExpectEmptyList()
        {
            // Arrange
            const int notExistGameId = 1;
            using var context = new ContextTest(Options);
            context.PlatformTypes.Add(CreatePlatformType());
            context.SaveChanges();
            var platformTypeService = GetPlatformTypeService(context);

            // Act
            var result = platformTypeService.GetByGame(notExistGameId);

            // Assert
            Assert.Empty(result);
        }
    }
}

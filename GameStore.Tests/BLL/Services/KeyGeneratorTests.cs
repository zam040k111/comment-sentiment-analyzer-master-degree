using GameStore.BLL.Services;
using GameStore.DAL;
using GameStore.Tests.DAL;
using Xunit;

namespace GameStore.Tests.BLL.Services
{
    public class KeyGeneratorTests : BllTests
    {
        [Fact]
        public void GenerateKey_WhenThereIsSameKeyWithoutNumber_ExpectNewKeyWithNumberInEnd()
        {
            // Arrange
            const string gameKey = "key";
            const string gameName = "name";
            using var context = new ContextTest(Options);
            context.Games.Add(CreateGame(gameKey, gameName));
            context.SaveChanges();
            var unitOfWork = new UnitOfWork(context, null, null);

            // Act
            var result = KeyGenerator.GenerateKey(CreateGameDto(name: gameKey), unitOfWork.GameRepository);

            // Assert
            Assert.Equal("key1", result);
        }

        [Fact]
        public void GenerateKey_WhenThereAreNoSameKeys_ExpectNewKeyAsGameNamePlusOne()
        {
            // Arrange
            const string gameKey = "key";
            const string gameName = "name";
            using var context = new ContextTest(Options);
            context.Games.Add(CreateGame("someKey", gameName));
            context.SaveChanges();
            var unitOfWork = new UnitOfWork(context, null, null);

            // Act
            var result = KeyGenerator.GenerateKey(CreateGameDto(name: gameKey), unitOfWork.GameRepository);

            // Assert
            Assert.Equal("key1", result);
        }

        [Fact]
        public void GenerateKey_WhenThereAreSameKeysWithoutNumber_ExpectNewKeyAsGameNamePlusOne()
        {
            // Arrange
            const string gameKey = "key";
            const string gameName = "name";
            using var context = new ContextTest(Options);
            context.Games.Add(CreateGame("KeyName", gameName));
            context.SaveChanges();
            var unitOfWork = new UnitOfWork(context, null, null);

            // Act
            var result = KeyGenerator.GenerateKey(CreateGameDto(name: gameKey), unitOfWork.GameRepository);

            // Assert
            Assert.Equal("key1", result);
        }

        [Fact]
        public void GenerateKey_WhenThereAreSameKeys_ExpectNewKeyWithBiggestNumberPlusOneInEnd()
        {
            // Arrange
            const string gameKey = "key";
            const string gameKey1 = "key1";
            const string gameKey2 = "key36";
            const string gameKey3 = "key30";
            const string gameName = "name";
            using var context = new ContextTest(Options);
            context.Games.Add(CreateGame(gameKey, gameName));
            context.Games.Add(CreateGame(gameKey1, gameName));
            context.Games.Add(CreateGame(gameKey2, gameName));
            context.Games.Add(CreateGame(gameKey3, gameName));
            context.SaveChanges();
            var unitOfWork = new UnitOfWork(context, null, null);

            // Act
            var result = KeyGenerator.GenerateKey(CreateGameDto(name: gameKey), unitOfWork.GameRepository);

            // Assert
            Assert.Equal("key37", result);
        }

        [Fact]
        public void GenerateKey_WhenThereAreSameKeysWithOtherLetter_ExpectNewKeyWithBiggestNumberPlusOneInEndWithoutOtherLetter()
        {
            // Arrange
            const string gameKey = "key";
            const string gameKey1 = "keyNAME8";
            const string gameName = "name";
            using var context = new ContextTest(Options);
            context.Games.Add(CreateGame(gameKey, gameName));
            context.Games.Add(CreateGame(gameKey1, gameName));
            context.SaveChanges();
            var unitOfWork = new UnitOfWork(context, null, null);

            // Act
            var result = KeyGenerator.GenerateKey(CreateGameDto(name: gameKey), unitOfWork.GameRepository);

            // Assert
            Assert.Equal("key9", result);
        }
    }
}

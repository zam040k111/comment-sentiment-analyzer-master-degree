using GameStore.BLL.Exceptions.ServiceExceptions;
using GameStore.Tests.DAL;
using Xunit;

namespace GameStore.Tests.BLL.Services
{
    public class PublisherServiceTests : BllTests
    {
        [Fact]
        public void Add_WhenEntityValid_ExpectEntityIsValidResult()
        {
            // Arrange
            const string name = "publisher";
            using var context = new ContextTest(Options);
            var publisherService = GetPublisherService(context);

            // Act
            var result = publisherService.Add(CreatePublisherDto(name));

            // Assert
            var expectedEntity = context.Publishers.Find(result.Value.Id);
            Assert.True(result.IsValid);
            Assert.NotNull(expectedEntity);
            Assert.Equal(expectedEntity.CompanyName, name);
        }

        [Fact]
        public void Add_WhenEntityNotValid_ExpectResultWithError()
        {
            // Arrange
            const string name = "publisher";
            const int id = 2;
            using var context = new ContextTest(Options);
            context.Publishers.Add(CreatePublisher(name));
            context.SaveChanges();
            using var contextForAdd = new ContextTest(Options);
            var publisherService = GetPublisherService(contextForAdd);

            // Act
            var result = publisherService.Add(CreatePublisherDto(name, id: id));

            // Assert
            Assert.True(!result.IsValid);
            Assert.True(result.Errors.ContainsKey("CompanyName"));
        }

        [Fact]
        public void Update_WhenEntityExist_ExpectEntityIsValidResult()
        {
            // Arrange
            const string updatedName = "new name";
            using var context = new ContextTest(Options);
            var publisher = context.Publishers.Add(CreatePublisher()).Entity;
            context.SaveChanges();
            using var contextForUpdate = new ContextTest(Options);
            var publisherService = GetPublisherService(contextForUpdate);

            // Act
            var result = publisherService.Update(CreatePublisherDto(updatedName, id: publisher.Id));

            // Assert
            var publisherActual = contextForUpdate.Publishers.Find(result.Value.Id);
            Assert.True(result.IsValid);
            Assert.NotNull(publisherActual);
            Assert.Equal(updatedName, publisherActual.CompanyName);
        }

        [Fact]
        public void Update_WhenEntityNotValid_ExpectResultWithError()
        {
            // Arrange
            const string name = "new publisher";
            const int id = 2;
            using var context = new ContextTest(Options);
            context.Publishers.Add(CreatePublisher(name));
            context.SaveChanges();
            using var contextForUpdate = new ContextTest(Options);
            var publisherService = GetPublisherService(contextForUpdate);

            // Act
            var result = publisherService.Update(CreatePublisherDto(name, id: id));

            // Assert
            Assert.True(!result.IsValid);
            Assert.True(result.Errors.ContainsKey("CompanyName"));
        }

        [Fact]
        public void Delete_WhenEntityExist_ExpectEntityIsDeleted()
        {
            // Arrange
            const string publisherName = "Publisher name";
            using var context = new ContextTest(Options);
            var publisher = context.Publishers.Add(CreatePublisher(publisherName)).Entity;
            context.SaveChanges();
            using var contextForDelete = new ContextTest(Options);
            var publisherService = GetPublisherService(contextForDelete);

            // Act
            publisherService.Delete(publisher.Id);

            // Assert
            Assert.True(contextForDelete.Publishers.Find(publisher.Id).IsDeleted);
        }

        [Fact]
        public void Delete_WhenEntityNotExist_ExpectNotFoundException()
        {
            // Arrange
            using var context = new ContextTest(Options);
            var publisherService = GetPublisherService(context);

            // Act

            // Assert
            Assert.Throws<NotFoundException>(() => publisherService.Delete(1));
        }

        [Fact]
        public void GetAll_WhenEmptiesNotExist_ExpectEmptyList()
        {
            // Arrange
            using var context = new ContextTest(Options);
            var publisherService = GetPublisherService(context);

            // Act
            var publishers = publisherService.GetAll();

            // Assert
            Assert.Empty(publishers);
        }

        [Fact]
        public void GetAll_WhenEmptiesExist_ExpectNotEmptyList()
        {
            // Arrange
            using var context = new ContextTest(Options);
            context.Publishers.Add(CreatePublisher());
            context.SaveChanges();
            var publisherService = GetPublisherService(context);

            // Act
            var publishers = publisherService.GetAll();

            // Assert
            Assert.NotEmpty(publishers);
        }

        [Fact]
        public void GetById_WhenEmptyExist_ExpectEntity()
        {
            // Arrange
            using var context = new ContextTest(Options);
            var publisherEntity = context.Publishers.Add(CreatePublisher()).Entity;
            context.SaveChanges();
            var publisherService = GetPublisherService(context);

            // Act
            var publisher = publisherService.GetById(publisherEntity.Id);

            // Assert
            Assert.NotNull(publisher);
            Assert.Equal(publisher.CompanyName, publisherEntity.CompanyName);
        }

        [Fact]
        public void GetById_WhenEmptyNotExist_ExpectNotFoundException()
        {
            // Arrange
            const int id = 1;
            using var context = new ContextTest(Options);
            var publisherService = GetPublisherService(context);

            // Act

            // Assert
            Assert.Throws<NotFoundException>(() => publisherService.GetById(id));
        }
    }
}

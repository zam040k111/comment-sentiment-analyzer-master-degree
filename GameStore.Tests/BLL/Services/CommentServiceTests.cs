using GameStore.Tests.DAL;
using Xunit;

namespace GameStore.Tests.BLL.Services
{
    public class CommentServiceTests : BllTests
    {
        [Fact]
        public void Add_WhenEntityValid_ExpectEntity()
        {
            // Arrange
            const string commentName = "max";
            using var context = new ContextTest(Options);
            var commentService = GetCommentService(context);

            // Act
            var result = commentService.Add(CreateCommentDto(commentName));

            // Assert
            var expectedEntity = context.Comments.Find(result.Id);
            Assert.NotNull(expectedEntity);
            Assert.Equal(expectedEntity.Name, commentName);
        }

        [Fact]
        public void Update_WhenEntityExist_ExpectEntity()
        {
            // Arrange
            const string commentName = "name";
            const string updatedCommentName = "new name";
            using var context = new ContextTest(Options);
            var comment = context.Comments.Add(CreateComment(commentName)).Entity;
            context.SaveChanges();
            using var contextForUpdate = new ContextTest(Options);
            var commentService = GetCommentService(contextForUpdate);

            // Act
            var result = commentService.Update(CreateCommentDto(updatedCommentName, id: comment.Id));

            // Assert
            var commentActual = contextForUpdate.Comments.Find(result.Id);
            Assert.NotNull(commentActual);
            Assert.Equal(updatedCommentName, commentActual.Name);
        }

        [Fact]
        public void Delete_WhenEntityExist_ExpectEntityIsDeleted()
        {
            // Arrange
            const string commentName = "comment";
            using var context = new ContextTest(Options);
            var comment = context.Comments.Add(CreateComment(commentName)).Entity;
            context.SaveChanges();
            using var contextForDelete = new ContextTest(Options);
            var commentService = GetCommentService(contextForDelete);

            // Act
            commentService.Delete(comment.Id);

            // Assert
            Assert.True(contextForDelete.Comments.Find(comment.Id).IsDeleted);
        }

        [Fact]
        public void GetAll_CommentsNotExist_ExpectEmptyList()
        {
            // Arrange
            using var context = new ContextTest(Options);
            var commentService = GetCommentService(context);

            // Act
            var comments = commentService.GetAll();

            // Assert
            Assert.Empty(comments);
        }

        [Fact]
        public void GetAll_CommentsExist_ExpectNotEmptyList()
        {
            // Arrange
            const string commentName = "Ivan";
            using var context = new ContextTest(Options);
            context.Comments.Add(CreateComment(commentName));
            context.SaveChanges();
            var commentService = GetCommentService(context);

            // Act
            var comments = commentService.GetAll();

            // Assert
            Assert.NotEmpty(comments);
        }
    }
}

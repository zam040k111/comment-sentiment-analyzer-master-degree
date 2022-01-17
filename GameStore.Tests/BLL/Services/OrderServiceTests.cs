using System.Collections.Generic;
using GameStore.BLL.DTO;
using GameStore.Tests.DAL;
using Xunit;

namespace GameStore.Tests.BLL.Services
{
    public class OrderServiceTests : BllTests
    {
        [Fact]
        public void Delete_WhenEntityExist_ExpectEntityIsDeleted()
        {
            // Arrange
            const string customerId = "customerId";
            using var context = new ContextTest(Options);
            var order = context.Orders.Add(CreateOrder(customerId)).Entity;
            context.SaveChanges();
            using var contextForDelete = new ContextTest(Options);
            var orderService = GetOrderService(contextForDelete);

            // Act
            orderService.Delete(order.Id);

            // Assert
            Assert.True(contextForDelete.Orders.Find(order.Id).IsDeleted);
        }

        [Fact]
        public void Add_WhenOrderDetailsNull_ExpectErrorWithOrderDetailsKey()
        {
            // Arrange
            using var context = new ContextTest(Options);
            var orderService = GetOrderService(context);

            // Act
            var result = orderService.Add(CreateOrderDto());

            // Assert
            Assert.True(!result.IsValid);
            Assert.True(result.Errors.ContainsKey("OrderDetails"));
        }

        [Fact]
        public void Add_WhenUnitInStockNotEnough_ExpectErrorWithTotalPrice()
        {
            // Arrange
            using var context = new ContextTest(Options);
            var orderService = GetOrderService(context);

            // Act
            var result = orderService.Add(CreateOrderDto(
                orderDetails: new List<OrderDetailDto>
                {
                    new OrderDetailDto
                    {
                        Product = new GameDto{ UnitsInStock = 1},
                        Quantity = 2
                    }
                }));

            // Assert
            Assert.True(!result.IsValid);
            Assert.True(result.Errors.ContainsKey("TotalPrice"));
        }

        [Fact]
        public void Add_WhenUnitInStockEnough_ExpectModelIsValid()
        {
            // Arrange
            using var context = new ContextTest(Options);
            var entity = context.Games.Add(CreateGame()).Entity;
            context.SaveChanges();
            var game = CreateGameDto();
            game.Id = entity.Id;
            game.UnitsInStock = 1;
            var order = CreateOrderDto(
                orderDetails: new List<OrderDetailDto>
                {
                    new OrderDetailDto
                    {
                        Product = game,
                        Quantity = 1
                    }
                });

            using var contextForAdd = new ContextTest(Options);
            var orderService = GetOrderService(contextForAdd);

            // Act
            var result = orderService.Add(order);
            var newOrder = context.Orders.Find(result.Value.Id);

            // Assert
            Assert.NotNull(newOrder);
            Assert.True(result.IsValid);
        }
    }
}

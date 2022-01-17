using System.Collections.Generic;
using GameStore.BLL.DTO;
using GameStore.Tests.WEB.Services.Fakes;
using GameStore.WEB.Extensions;
using GameStore.WEB.Services;
using Xunit;

namespace GameStore.Tests.WEB.Services
{
    public class CartServiceTests : WebTests
    {
        [Fact]
        public void GetAll_WhenCartNotEmpty_ExpectOrderDtoEntity()
        {
            // Arrange
            const string customerId = "Sasha";
            var session = new FakeSession();
            session.Set("cart", new OrderDto {CustomerId = customerId});
            var cartService = new CartService();

            // Act
            var result = cartService.GetAll(session);

            // Assert
            Assert.IsType<OrderDto>(result);
            Assert.Equal(customerId, result.CustomerId);
        }

        [Fact]
        public void Remove_WhenEntityExist_ExpectEntityDeleted()
        {
            // Arrange
            const string key = "1";
            const string key2 = "2";
            var session = new FakeSession();
            session.Set("cart", new OrderDto{
                OrderDetails = new List<OrderDetailDto>
            {
                new OrderDetailDto{ProductKey = key},
                new OrderDetailDto{ProductKey = key2}
            }});
            var cartService = new CartService();

            // Act
            var newSession = cartService.Remove(key2, session);
            var cartValue = cartService.GetAll(newSession).OrderDetails;

            // Assert
            Assert.Null(cartValue.Find(i => i.ProductKey == key2));
            Assert.NotNull(cartValue.Find(i => i.ProductKey == key));
        }

        [Fact]
        public void Remove_WhenEntityNotExist_ExpectNoChange()
        {
            // Arrange
            const string key = "1";
            var session = new FakeSession();
            var order = new OrderDto
            {
                OrderDetails = new List<OrderDetailDto>
                {
                    new OrderDetailDto {ProductKey = key}
                }
            };

            session.Set("cart", order);
            var cartService = new CartService();

            // Act
            var newSession = cartService.Remove("2", session);
            var cartValue = cartService.GetAll(newSession).OrderDetails;

            // Assert
            Assert.Equal(order.OrderDetails.Count,cartValue.Count);
        }

        [Fact]
        public void Add_WhenEntityNotExist_ExpectElementInCart()
        {
            // Arrange
            const string key = "1";
            const string key2 = "2";
            const string key3 = "3";
            var session = new FakeSession();
            var order = new OrderDto
            {
                OrderDetails = new List<OrderDetailDto>
                {
                    new OrderDetailDto {ProductKey = key},
                    new OrderDetailDto {ProductKey = key2}
                }
            };

            session.Set("cart", order);
            var cartService = new CartService();

            // Act
            var newSession = cartService.Add(new GameDto{Key = key3}, session);
            var orderDetails = cartService.GetAll(newSession).OrderDetails;

            // Assert
            Assert.Equal(3, orderDetails.Count);
        }

        [Fact]
        public void UpdateQuantity_Always_ExpectOrderWithUpdatedQuantity()
        {
            // Arrange
            const int quantity = 1;
            const int newQuantity = 2;
            var session = new FakeSession();
            var order = new OrderDto
            {
                OrderDetails = new List<OrderDetailDto>
                {
                    new OrderDetailDto {Quantity = quantity}
                }
            };

            session.Set("cart", order);
            var cartService = new CartService();

            // Act
            var newOrder = cartService.UpdateQuantity(session, 0, newQuantity);

            // Assert
            Assert.Equal(newQuantity, newOrder.OrderDetails[0].Quantity);
        }
    }
}

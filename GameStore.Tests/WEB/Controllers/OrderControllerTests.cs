using System.IO;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Exceptions.ServiceExceptions;
using GameStore.BLL.Interfaces;
using GameStore.WEB.Controllers;
using GameStore.WEB.Interfaces;
using GameStore.WEB.Mapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GameStore.Tests.WEB.Controllers
{
    public class OrderControllerTests : WebTests
    {
        private readonly Mock<IOrderService> _orderService;
        private readonly Mock<ICartService> _cartService;

        public OrderControllerTests()
        {
            _orderService = new Mock<IOrderService>();
            _cartService = new Mock<ICartService>();
        }

        [Fact]
        public void Download_WhenOrderExist_ExpectFileStreamResult()
        {
            // Arrange
            const int id = 1;
            _orderService.Setup(i => i.GetById(It.IsAny<int>())).Returns(new OrderDto{Id = id});
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Content");
            Directory.CreateDirectory(path);
            var controller = new OrderController(_orderService.Object,_cartService.Object, Mapper);

            // Act
            var result = controller.Download(id);

            // Assert
            Assert.IsType<FileStreamResult>(result);

            File.Delete(Path.Combine(path, "Order1.pdf"));
            Directory.Delete(path);
        }

        [Fact]
        public void Download_WhenOrderNotExist_ExpectNotFoundException()
        {
            // Arrange
            const int id = 1;
            _orderService.Setup(i => i.GetById(It.IsAny<int>())).Returns((OrderDto) null);
            var controller = new OrderController(_orderService.Object, _cartService.Object, Mapper);

            // Act

            // Assert
            Assert.Throws<NotFoundException>(() => controller.Download(id));
        }
    }
}

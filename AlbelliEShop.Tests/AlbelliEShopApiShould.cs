using AlbelliEShop.Core;
using AlbelliEShop.Core.Model;
using AlbelliEShop.Domain;
using AlbelliEShop.Persistence.Contract;
using AlbelliEShop.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace AlbelliEShop.Tests
{
    public class AlbelliEShopApiShould
    {
        public AlbelliEShopApiShould() { }

        #region GetAllOrdersTest
        [Fact]
        public void FetchOrderDetailsSuccessfully()
        {
            //Arrange
            var mock = new Mock<IOrderService>();
            mock.Setup(p => p.GetAllOrders()).Returns(new List<Order>());
            OrdersController order = new OrdersController(mock.Object);

            //Act
            var result = order.GetAllOrders();
            var okResult = result as OkObjectResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }
        #endregion GetAllOrdersTest

        #region GetOrderByIdTest
        [Fact]
        public void FetchOrderDetailsByIdSuccessfully()
        {
            //Arrange
            var mock = new Mock<IOrderService>();
            mock.Setup(p => p.GetOrderById(It.IsAny<string>())).Returns(new Order());
            OrdersController order = new OrdersController(mock.Object);

            //Act
            var result = order.GetOrder("abcd123");
            var okResult = result as OkObjectResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public void DisplayErrorMessageWhenIdIsNullForFetchingOrder()
        {
            //Arrange
            var mock = new Mock<IOrderService>();
            mock.Setup(p => p.GetOrderById(It.IsAny<string>())).Returns(new Order());
            OrdersController order = new OrdersController(mock.Object);

            //Act
            var result = order.GetOrder(null);
            var badRequestObjectResult = result as BadRequestObjectResult;

            //Assert
            Assert.Contains("Please enter Id", badRequestObjectResult.Value.ToString());
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }
        #endregion GetOrderByIdTest

        #region PlaceOrderTest
        [Fact]
        public void PlaceNewOrderSuccessfully()
        {
            //Arrange
            var mockOrderRequest = new OrderRequest()
            {
                Products = new List<Core.Model.Product> { new Core.Model.Product
                {
                    Name = "photobook",
                    Quantity = 2
                } }
            };
            var mockOrder = new Order()
            {
                Id = "mock_Id",
                Products = new List<Domain.Product> { new Domain.Product
                {
                    ProductName = "photobook",
                    Quantity = 2,
                } },
                RequiredBinWidthInMillimeters = 38
            };
            var mock = new Mock<IOrderService>();
            mock.Setup(p => p.PlaceOrder(It.IsAny<Order>())).Returns(mockOrder);
            OrdersController order = new OrdersController(mock.Object);

            //Act
            var result = order.PlaceOrder(mockOrderRequest);
            var okResult = result as OkObjectResult;
            var item = okResult.Value as OrderResponse;

            //Assert
            Assert.IsType<OrderResponse>(item);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public void NotPlaceOrderWhenProductTypeIsInvalid()
        {
            //Arrange
            var mockOrder = new Order()
            {
                Id = "mock_Id",
                Products = new List<Domain.Product> { new Domain.Product
                {
                    ProductName = "invalid_product",
                    Quantity = 2,
                } },
                RequiredBinWidthInMillimeters = 0
            };
            var mockOrderRepository = new Mock<IOrderRepository>();
            mockOrderRepository.Setup(p => p.AddOrderInDb(It.IsAny<Order>())).Returns(new Order());
            OrderService orderService = new OrderService(mockOrderRepository.Object);

            //Act
            var result = orderService.PlaceOrder(mockOrder);

            //Assert
            Assert.Null(result);
        }
        [Fact]
        public void CalculateBinWidthWhilePlacingOrder()
        {
            //Arrange
            var mockOrder = new Order()
            {
                Id = "mock_Id",
                Products = new List<Domain.Product> { new Domain.Product
                {
                    ProductName = "mug",
                    Quantity = 2,
                } },
                RequiredBinWidthInMillimeters = 94
            };
            var mockOrderRepository = new Mock<IOrderRepository>();
            mockOrderRepository.Setup(p => p.AddOrderInDb(It.IsAny<Order>())).Returns(new Order());
            OrderService orderService = new OrderService(mockOrderRepository.Object);

            //Act
            var result = orderService.PlaceOrder(mockOrder);

            //Assert
            Assert.NotNull(result);
        }
        #endregion PlaceOrderTest
    }
}

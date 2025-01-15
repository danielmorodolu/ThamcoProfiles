using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProfileService.Controllers;
using ProfileService.Models;
using ProfileService.Services.Products;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Xunit;

namespace ProfileService.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<ICompositeViewEngine> _mockViewEngine;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _mockProductService = new Mock<IProductService>();
            _mockViewEngine = new Mock<ICompositeViewEngine>();

            // Properly mock ViewEngine behavior
            _mockViewEngine.Setup(engine => engine.FindView(It.IsAny<ControllerContext>(), It.IsAny<string>(), false))
                           .Returns(ViewEngineResult.Found("TestView", Mock.Of<IView>()));

            _controller = new HomeController(_mockLogger.Object, _mockProductService.Object, _mockViewEngine.Object);
        }

        [Fact]
        public async Task Search_ReturnsFilteredProducts_WhenQueryIsProvided()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product A" },
                new ProductDto { Id = 2, Name = "Another Product" }
            };

            _mockProductService.Setup(service => service.GetProductsAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _controller.Search("Product") as ViewResult;
            var model = result?.Model as IEnumerable<ProductDto>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Equal(2, model?.Count());
        }

        [Fact]
        public async Task Search_ReturnsEmptyList_WhenQueryDoesNotMatch()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product A" },
                new ProductDto { Id = 2, Name = "Another Product" }
            };

            _mockProductService.Setup(service => service.GetProductsAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _controller.Search("NonExistent") as ViewResult;
            var model = result?.Model as IEnumerable<ProductDto>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Empty(model ?? Enumerable.Empty<ProductDto>());
        }
    }
}

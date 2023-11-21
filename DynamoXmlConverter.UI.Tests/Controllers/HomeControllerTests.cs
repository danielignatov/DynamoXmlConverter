using DynamoXmlConverter.UI.Controllers;
using DynamoXmlConverter.UI.Models.Home;
using DynamoXmlConverter.UI.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DynamoXmlConverter.UI.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly IHttpClientFactory _httpClientFactory;
        readonly HomeController _controller;

        public HomeControllerTests()
        {
            _httpClientFactory = new MockHttpClientFactory();
            _controller = new HomeController(_httpClientFactory);
        }

        [Fact]
        public void Index_ReturnsAViewResult_Success()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);
        }
    }
}
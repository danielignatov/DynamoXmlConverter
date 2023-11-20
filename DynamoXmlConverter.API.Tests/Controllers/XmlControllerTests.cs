using DynamoXmlConverter.API.Controllers;
using DynamoXmlConverter.API.Services;
using DynamoXmlConverter.API.Tests.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynamoXmlConverter.API.Tests.Controllers
{
    public class XmlControllerTests
    {
        readonly IWebHostEnvironment _hostingEnvironment;
        readonly IFileService _fileService;
        readonly XmlController _controller;

        public XmlControllerTests()
        {
            _hostingEnvironment = new MockWebHostEnvironment();
            _fileService = new FileService(_hostingEnvironment);
            _controller = new XmlController(_fileService);
        }

        [Fact]
        public void Upload_NoFiles_BadRequest()
        {
            // Arrange
            var files = new List<IFormFile>();

            // Act
            var result = _controller.Upload(files);
            var resultType = result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(resultType?.StatusCode == 400);
        }

        [Fact]
        public void Upload_WrongFileType_BadRequest()
        {
            // Arrange
            IFormFile file = TestingUtils.CreateIFormFileFile("test.pdf", Constants.HELLO_WORLD);
            var files = new List<IFormFile>() { file };

            // Act
            var result = _controller.Upload(files);
            var resultType = result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(resultType?.StatusCode == 400);
        }

        [Fact]
        public void Upload_CorrectFileType_OkResult()
        {
            // Arrange
            _fileService.DeleteFile("test3.json");
            IFormFile file = TestingUtils.CreateIFormFileFile("test3.xml", Constants.XML_CONTENT_EXAMPLE_GOOD);
            var files = new List<IFormFile>() { file };

            // Act
            var result = _controller.Upload(files);
            var resultType = result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(resultType?.StatusCode == 200);
        }

        [Fact]
        public void Upload_IncorrectlyFormattedFile_BadRequest()
        {
            // Arrange
            _fileService.DeleteFile("test4.json");
            IFormFile file = TestingUtils.CreateIFormFileFile("test4.xml", Constants.XML_CONTENT_EXAMPLE_BAD);
            var files = new List<IFormFile>() { file };

            // Act
            var result = _controller.Upload(files);
            var resultType = result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(resultType?.StatusCode == 400);
        }
    }
}
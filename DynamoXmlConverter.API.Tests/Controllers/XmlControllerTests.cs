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
            // Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            // Create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

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
            _fileService.DeleteFile("test.json");

            // Setup mock file using a memory stream
            var content = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><note>  <to>Tove</to>  <from>Jani</from>  <heading>Reminder</heading>  <body>Don't forget me this weekend!</body></note>";
            var fileName = "test.xml";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            // Create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

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

            _fileService.DeleteFile("test.json");

            // Setup mock file using a memory stream
            var content = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><note>  <to>Tove</to>  <from>Jani</from>  <heading>Reminder</heading>  <body>Don't forget me this weekend!";
            var fileName = "test.xml";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            // Create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

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
using DynamoXmlConverter.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynamoXmlConverter.Tests
{
    public class XmlControllerTests
    {
        private const string WEB_ROOT_PATH = "C:\\Users\\danie\\Documents\\GitHub\\DynamoXmlConverter\\DynamoXmlConverter\\wwwroot";

        readonly XmlController _controller;
        readonly IWebHostEnvironment _hostingEnvironment;

        public XmlControllerTests()
        {
            _hostingEnvironment = new MockWebHostEnvironment(WEB_ROOT_PATH);
            _controller = new XmlController(_hostingEnvironment);
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
            Assert.True(resultType?.Value?.ToString() == "No file uploaded.");
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
            Assert.True(resultType?.Value?.ToString() == "Incorrect file type extension. Expected: .xml");
        }

        [Fact]
        public void Upload_CorrectFileType_OkResult()
        {
            // Arrange
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
            Assert.True(resultType?.Value?.ToString() == "Invalid .xml file.");
        }
    }
}
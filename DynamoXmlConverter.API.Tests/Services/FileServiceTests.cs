using DynamoXmlConverter.API.Services;
using DynamoXmlConverter.API.Tests.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DynamoXmlConverter.API.Tests.Services
{
    public class FileServiceTests
    {
        readonly IWebHostEnvironment _hostingEnvironment;
        readonly IFileService _fileService;

        public FileServiceTests()
        {
            _hostingEnvironment = new MockWebHostEnvironment();
            _fileService = new FileService(_hostingEnvironment);
        }

        [Fact]
        public void DeleteFile_ExistingFile_Success()
        {
            // Arrange
            var fileName = "test1.json";

            TestingUtils.CreateFileIfDoesNotExist(fileName);

            // Act
            var result = _fileService.DeleteFile(fileName);
            
            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DeleteFile_NonExistingFile_False()
        {
            // Arrange
            var fileName = "test2.json";

            TestingUtils.DeleteFileIfExist(fileName);

            // Act
            var result = _fileService.DeleteFile(fileName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ProcessXmlFiles_SingleFile_Success()
        {
            // Arrange
            TestingUtils.DeleteFileIfExist("test5.json");

            IFormFile file = TestingUtils.CreateIFormFileFile("test5.xml", Constants.XML_CONTENT_EXAMPLE_GOOD);

            var files = new List<IFormFile>() { file };

            // Act
            var results = _fileService.ProcessXmlFiles(files);

            // Assert
            Assert.Contains(results, x => x.Success == true);
        }

        [Fact]
        public void ProcessXmlFiles_MultipleFiles_Success()
        {
            // Arrange
            TestingUtils.DeleteFileIfExist("test5.json");
            TestingUtils.DeleteFileIfExist("test6.json");

            IFormFile file1 = TestingUtils.CreateIFormFileFile("test5.xml", Constants.XML_CONTENT_EXAMPLE_GOOD);
            IFormFile file2 = TestingUtils.CreateIFormFileFile("test6.xml", Constants.XML_CONTENT_EXAMPLE_GOOD);

            var files = new List<IFormFile>() { file1, file2 };

            // Act
            var results = _fileService.ProcessXmlFiles(files);

            // Assert
            Assert.DoesNotContain(results, x => x.Success == false);
        }

        [Fact]
        public void ProcessXmlFiles_OneGoodTwoBad_CorrectResults()
        {
            // Arrange
            TestingUtils.DeleteFileIfExist("test7.json");
            TestingUtils.DeleteFileIfExist("test8.json");
            TestingUtils.DeleteFileIfExist("test9.json");

            IFormFile file1 = TestingUtils.CreateIFormFileFile("test7.xml", Constants.XML_CONTENT_EXAMPLE_GOOD);
            IFormFile file2 = TestingUtils.CreateIFormFileFile("test8.xml", Constants.XML_CONTENT_EXAMPLE_BAD);
            IFormFile file3 = TestingUtils.CreateIFormFileFile("test9.pdf", Constants.HELLO_WORLD);

            var files = new List<IFormFile>() { file1, file2, file3 };

            // Act
            var results = _fileService.ProcessXmlFiles(files);

            int goodFileResultCount = results.Where(x => x.Success == true).Count();
            int badFileResultCount = results.Where(x => x.Success == false).Count();

            // Assert
            Assert.Equal(1, goodFileResultCount);
            Assert.Equal(2, badFileResultCount);
        }
    }
}
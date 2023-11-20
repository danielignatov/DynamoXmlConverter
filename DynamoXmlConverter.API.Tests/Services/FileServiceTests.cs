using DynamoXmlConverter.API.Services;
using DynamoXmlConverter.API.Tests.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var filePath = Path.Combine(Constants.WEB_ROOT_PATH, Constants.FILE_UPLOADS_FOLDER_NAME, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                using (File.Create(filePath))
                {
                }
            }

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
            var filePath = Path.Combine(Constants.WEB_ROOT_PATH, Constants.FILE_UPLOADS_FOLDER_NAME, fileName);

            if (System.IO.File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Act
            var result = _fileService.DeleteFile(fileName);

            // Assert
            Assert.False(result);
        }
    }
}

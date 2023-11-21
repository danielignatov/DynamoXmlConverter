using DynamoXmlConverter.API.Extensions;

namespace DynamoXmlConverter.API.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void XmlToPrettyJson_Convert_Success()
        {
            // Arrange
            var xmlString = Constants.XML_CONTENT_EXAMPLE_GOOD;

            // Act
            var result = xmlString.XmlToPrettyJson();

            // Assert
            Assert.Equal("{\r\n  \"?xml\": {\r\n    \"@version\": \"1.0\",\r\n    \"@encoding\": \"UTF-8\"\r\n  },\r\n  \"note\": {\r\n    \"to\": \"Tove\",\r\n    \"from\": \"Jani\",\r\n    \"heading\": \"Reminder\",\r\n    \"body\": \"Don't forget me this weekend!\"\r\n  }\r\n}", result);
        }

        [Fact]
        public void SafelyWriteToFile_CreateFile_Success()
        {
            // Arrange
            var fileName = "test10.txt";
            var content = Constants.HELLO_WORLD;
            var filePath = Path.Combine(Constants.WEB_ROOT_PATH, Constants.FILE_UPLOADS_FOLDER_NAME, fileName);

            TestingUtils.DeleteFileIfExist(fileName);

            // Act
            content.SafelyWriteToFile(filePath, false);

            // Assert
            Assert.True(System.IO.File.Exists(filePath));
        }
    }
}
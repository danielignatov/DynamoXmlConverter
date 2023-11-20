using Microsoft.AspNetCore.Http;

namespace DynamoXmlConverter.API.Tests
{
    internal static class TestingUtils
    {
        /// <summary>
        /// Delete a file if exists
        /// </summary>
        /// <param name="fileName">File name including extension</param>
        internal static void DeleteFileIfExist(string fileName)
        {
            var filePath = Path.Combine(Constants.WEB_ROOT_PATH, Constants.FILE_UPLOADS_FOLDER_NAME, fileName);

            if (System.IO.File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// Creates a file if does not exists
        /// </summary>
        /// <param name="fileName">File name including extension</param>
        internal static void CreateFileIfDoesNotExist(string fileName)
        {
            var filePath = Path.Combine(Constants.WEB_ROOT_PATH, Constants.FILE_UPLOADS_FOLDER_NAME, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                using (File.Create(filePath))
                {
                }
            }
        }

        /// <summary>
        /// Setup mock file using a memory stream
        /// </summary>
        /// <param name="fileName">File name including extension</param>
        /// <param name="content">File content</param>
        internal static IFormFile CreateIFormFileFile(string fileName, string content)
        {
            // Setup mock file using a memory stream
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            // Create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
            return file;
        }
    }
}
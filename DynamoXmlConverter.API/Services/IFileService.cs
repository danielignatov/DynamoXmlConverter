using DynamoXmlConverter.Models.Results;

namespace DynamoXmlConverter.API.Services
{
    /// <summary>
    /// Service governing file related operations
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Process group of XML files, converting them to JSON, and then save them in a pre-defined directory.
        /// </summary>
        public List<FileUploadResult> ProcessXmlFiles(List<IFormFile> files);

        /// <summary>
        /// Deletes a file by given file name
        /// </summary>
        /// <param name="fileName">File name, including extension</param>
        public bool DeleteFile(string fileName);

        public List<string> AllUploadedJsonFileNames();

        public Task<FileResult> GetFile(string fileName);
    }
}
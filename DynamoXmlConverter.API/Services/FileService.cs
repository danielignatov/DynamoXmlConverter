using DynamoXmlConverter.API.Extensions;
using DynamoXmlConverter.Models.Results;
using System.Collections.Concurrent;
using System.Xml;

namespace DynamoXmlConverter.API.Services
{
    /// <summary>
    /// Service governing file related operations
    /// </summary>
    public class FileService : IFileService
    {
        private const string FILE_UPLOADS_FOLDER_NAME = "uploads";

        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Process group of XML files, converting them to JSON, and then save them in a pre-defined directory.
        /// </summary>
        public List<FileUploadResult> ProcessXmlFiles(List<IFormFile> files)
        {
            // https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-write-a-simple-parallel-foreach-loop

            var fileUploadResults = new ConcurrentBag<FileUploadResult>();

            Parallel.ForEach(files, file =>
            {
                var result = UploadXmlFile(file);
                fileUploadResults.Add(result);
            });

            return fileUploadResults.ToList();
        }

        /// <summary>
        /// Deletes a file by given file name
        /// </summary>
        /// <param name="fileName">File name, including extension</param>
        public bool DeleteFile(string fileName)
        {
            try
            {
                var fileFolderPath = GetFileFolderPath();
                var filePath = Path.Combine(fileFolderPath, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return false;
                }

                System.IO.File.Delete(filePath);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Process XML file into JSON
        /// </summary>
        private FileUploadResult UploadXmlFile(IFormFile file)
        {
            var result = new FileUploadResult();

            // Validation
            if (file == null || file.Length == 0)
            {
                result.Message = Messages.NoFileToUpload;
                return result;
            }

            result.FileName = file.FileName;

            var extension = Path.GetExtension(file.FileName);

            if (extension.ToLower() != ".xml")
            {
                result.Message = Messages.IncorrectFileExtensionXmlExpected;
                return result;
            }

            string fileFolderPath = GetFileFolderPath();

            if (!Directory.Exists(fileFolderPath))
                Directory.CreateDirectory(fileFolderPath);

            var fileNameNoExt = Path.GetFileNameWithoutExtension(file.FileName);

            var filePath = Path.Combine(fileFolderPath, fileNameNoExt + ".json");

            if (System.IO.File.Exists(filePath))
            {
                result.Message = Messages.FileAlreadyUploaded;
                return result;
            }

            // Read the xml file
            string xmlString;

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                xmlString = reader.ReadToEnd();
            }

            // Write the json file
            try
            {
                string json = xmlString.XmlToPrettyJson();
                json.SafelyWriteToFile(filePath, false);
            }
            catch (XmlException)
            {
                result.Message = Messages.InvalidXmlFile;
                return result;
            }

            result.Success = true;
            result.Message = Messages.FileUploadedSuccessfully;
            return result;
        }

        /// <summary>
        /// Retreve the folder path where the files are stored
        /// </summary>
        private string GetFileFolderPath()
        {
            return Path.Combine(_hostingEnvironment.WebRootPath, FILE_UPLOADS_FOLDER_NAME);
        }
    }
}
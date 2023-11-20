﻿using DynamoXmlConverter.API.Extensions;
using DynamoXmlConverter.Models.Results;
using System.Collections.Concurrent;
using System.Xml;

namespace DynamoXmlConverter.API.Services
{
    public class FileService : IFileService
    {
        private const string FILE_UPLOADS_FOLDER_NAME = "uploads";

        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        // https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-write-a-simple-parallel-foreach-loop
        public List<FileUploadResult> ProcessXmlFiles(List<IFormFile> files)
        {
            var fileUploadResults = new ConcurrentBag<FileUploadResult>();

            Parallel.ForEach(files, file =>
            {
                var result = UploadXmlFile(file);
                fileUploadResults.Add(result);
            });

            return fileUploadResults.ToList();
        }

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
            return result;
        }

        private string GetFileFolderPath()
        {
            return Path.Combine(_hostingEnvironment.WebRootPath, FILE_UPLOADS_FOLDER_NAME);
        }
    }
}
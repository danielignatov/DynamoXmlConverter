using DynamoXmlConverter.Extensions;
using DynamoXmlConverter.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Xml;

namespace DynamoXmlConverter.Controllers
{
    [ApiController]
    public class XmlController : ControllerBase
    {
        private const string FILE_UPLOADS_FOLDER_NAME = "uploads";

        private readonly IWebHostEnvironment _hostingEnvironment;

        public XmlController(
            IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("api/xml/upload")]
        public IActionResult Upload(List<IFormFile> files)
        {
            try
            {
                if (files.Count == 0)
                    return BadRequest("No file uploaded.");

                var results = UploadFiles(files);

                if (results.Any(x => x.Success == false))
                    return BadRequest(results);

                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong.");
            }
        }

        [HttpDelete]
        [Route("api/xml/delete")]
        public IActionResult Delete(string fileName)
        {
            try
            {
                var fileFolderPath = GetFileFolderPath();
                var filePath = Path.Combine(fileFolderPath, fileName + ".json");

                if (!System.IO.File.Exists(filePath))
                {
                    return BadRequest("File does not exist.");
                }

                System.IO.File.Delete(filePath);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong.");
            }
        }

        // https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-write-a-simple-parallel-foreach-loop
        private List<FileUploadResult> UploadFiles(List<IFormFile> files)
        {
            var fileUploadResults = new ConcurrentBag<FileUploadResult>();

            Parallel.ForEach(files, file =>
            {
                var result = UploadFile(file);
                fileUploadResults.Add(result);
            });

            return fileUploadResults.ToList();
        }

        private FileUploadResult UploadFile(IFormFile file)
        {
            var result = new FileUploadResult();

            // Validation
            if (file == null || file.Length == 0)
            {
                result.Message = "No file to upload.";
                return result;
            }

            result.FileName = file.FileName;

            var extension = Path.GetExtension(file.FileName);

            if (extension.ToLower() != ".xml")
            {
                result.Message = "Incorrect file type extension. Expected: .xml";
                return result;
            }

            string fileFolderPath = GetFileFolderPath();

            if (!Directory.Exists(fileFolderPath))
                Directory.CreateDirectory(fileFolderPath);

            var fileNameNoExt = Path.GetFileNameWithoutExtension(file.FileName);

            var filePath = Path.Combine(fileFolderPath, fileNameNoExt + ".json");

            if (System.IO.File.Exists(filePath))
            {
                result.Message = "File already uploaded.";
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
                result.Message = "Invalid .xml file.";
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
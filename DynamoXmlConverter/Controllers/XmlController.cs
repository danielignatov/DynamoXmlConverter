using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Xml.Linq;

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
        public async Task<IActionResult> Upload(IFormFile? file = null)
        {
            // Validation
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var extension = Path.GetExtension(file.FileName);

            if (extension.ToLower() != ".xml")
                return BadRequest("Incorrect file type extension. Expected: .xml");

            // Read the xml file
            string xmlString;

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                xmlString = await reader.ReadToEndAsync();
            }

            // Write the json file
            string fileFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, FILE_UPLOADS_FOLDER_NAME);

            if (!Directory.Exists(fileFolderPath))
                Directory.CreateDirectory(fileFolderPath);

            var filePath = Path.Combine(fileFolderPath, GetFileName() + ".json");

            await System.IO.File.WriteAllTextAsync(filePath, XmlToPrettyJson(xmlString));

            return Ok($"File created. {filePath}");
        }

        private static string GetFileName()
        {
            return Guid.NewGuid().ToString();
        }

        private static string XmlToPrettyJson(string xml)
        {
            var doc = XDocument.Parse(xml);
            return JsonConvert.SerializeXNode(doc, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
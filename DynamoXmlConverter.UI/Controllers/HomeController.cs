using DynamoXmlConverter.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mime;
using System.Text;

namespace DynamoXmlConverter.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("dynamoXmlConverterApi");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(IFormFile[] files)
        {
            try
            {
                using MultipartFormDataContent multipartContent = new();

                foreach (IFormFile file in files)
                {
                    multipartContent.Add(new StreamContent(file.OpenReadStream())
                    {
                        Headers =
                        {
                            ContentLength = file.Length,
                            ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType)
                        }
                    }, file.Name, file.FileName);
                }

                using var response = await _httpClient.PostAsync("https://localhost:5030/api/xml/upload", multipartContent);

                if (response.IsSuccessStatusCode)
                {
                    // Data uploaded successfully.
                }

                ViewBag.Message = "Selected Files are Upload successfully.";
            }
            catch
            {
                ViewBag.Message = "Error: Error occure for uploading a file.";
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
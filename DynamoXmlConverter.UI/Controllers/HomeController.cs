using DynamoXmlConverter.Models.Results;
using DynamoXmlConverter.UI.Models.Home;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace DynamoXmlConverter.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("dynamoXmlConverterApi");
        }

        public IActionResult Index()
        {
            var model = new IndexViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(IFormFile[] files)
        {
            var model = new IndexViewModel();

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

                var responseResultJsonString = await response.Content.ReadAsStringAsync();

                var responseResult = JsonConvert.DeserializeObject<UploadOperationResult>(responseResultJsonString) ?? new UploadOperationResult();

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        foreach (var file in responseResult.Files)
                        {
                            model.Alerts.Add(new Models.Shared.AlertViewModel() { AlertType = Models.Shared.AlertType.Success, Text = $"{file.FileName} {file.Message}" });
                        }
                        break;
                    case System.Net.HttpStatusCode.BadRequest:
                        model.Alerts.Add(new Models.Shared.AlertViewModel() { AlertType = Models.Shared.AlertType.Warning, Text = responseResult.Message });
                        foreach (var file in responseResult.Files)
                        {
                            model.Alerts.Add(new Models.Shared.AlertViewModel() { AlertType = file.Success ? Models.Shared.AlertType.Success : Models.Shared.AlertType.Warning, Text = $"{file.FileName} {file.Message}" });
                        }
                        break;
                    case System.Net.HttpStatusCode.InternalServerError:
                        model.Alerts.Add(new Models.Shared.AlertViewModel() { AlertType = Models.Shared.AlertType.Error, Text = responseResult.Message });
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                model.Alerts.Add(new Models.Shared.AlertViewModel() { AlertType = Models.Shared.AlertType.Error, Text = "Something went terribly wrong!" });
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.Shared.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
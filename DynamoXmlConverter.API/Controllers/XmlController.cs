using DynamoXmlConverter.API.Services;
using DynamoXmlConverter.Models.Results;
using Microsoft.AspNetCore.Mvc;

namespace DynamoXmlConverter.API.Controllers
{
    [ApiController]
    public class XmlController : ControllerBase
    {
        private readonly IFileService _fileService;

        public XmlController(
            IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        [Route("api/xml/upload")]
        [Produces("application/json")]
        public IActionResult Upload(List<IFormFile> files)
        {
            var result = new UploadOperationResult();

            try
            {
                if (files.Count == 0)
                {
                    result.Message = Messages.NoFilesUploaded;
                    return BadRequest(result);
                }
                
                var fileUploadResults = _fileService.ProcessXmlFiles(files);

                if (fileUploadResults.Any(x => x.Success == false))
                {
                    result.Message = Messages.IssueWithOneOrMoreFiles;
                    result.Files = fileUploadResults;
                    return BadRequest(result);
                }

                result.Message = Messages.FilesUploadedSuccessfully;
                result.Files = fileUploadResults;
                return Ok(result);
            }
            catch (Exception)
            {
                result.Message = Messages.SomethingWentWrong;
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }
    }
}
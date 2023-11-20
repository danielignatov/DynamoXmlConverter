using DynamoXmlConverter.API.Services;
using DynamoXmlConverter.Models.Results;
using Microsoft.AspNetCore.Mvc;

namespace DynamoXmlConverter.API.Controllers
{
    /// <summary>
    /// Endpoints for handling XML files
    /// </summary>
    [ApiController]
    public class XmlController : ControllerBase
    {
        private readonly IFileService _fileService;

        public XmlController(
            IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Endpoint for uploading XML files
        /// </summary>
        /// <param name="files">File attachments</param>
        /// <response code="200">File(s) uploaded successfully</response>
        /// <response code="400">No file(s) uploaded or there was an issue with one or more files</response>
        /// <response code="500">An unexpected error occurred on the server</response>
        [HttpPost]
        [Route("api/xml/upload")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UploadOperationResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UploadOperationResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UploadOperationResult), StatusCodes.Status500InternalServerError)]
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
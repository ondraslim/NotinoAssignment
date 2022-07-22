using Microsoft.AspNetCore.Mvc;
using WebApiVersion.Models;
using WebApiVersion.Services;

namespace WebApiVersion.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly ILogger<DocumentController> logger;
        private readonly IDocumentService documentService;

        public DocumentController(
            ILogger<DocumentController> logger,
            IDocumentService documentService)
        {
            this.logger = logger;
            this.documentService = documentService;
        }

        [HttpPost(Name = "Convert")]
        // TODO: add swagger, allow client generation for "safe" types
        public async Task<IActionResult> Convert(IFormFile file, FileType sourceType, FileType targetType)
        {
            logger.LogInformation($"{nameof(Convert)}({file.FileName})");

            if (sourceType is FileType.Unknown || targetType is FileType.Unknown)
            {
                // log
                return BadRequest();
            }

            if (sourceType == targetType)
            {
                // log
                return BadRequest();
            }

            try
            {
                using var sr = new StreamReader(file.OpenReadStream());
                var content = await sr.ReadToEndAsync();
                var documentDownload = documentService.ConvertDocument(content, sourceType, targetType);
                return File(documentDownload.Stream, documentDownload.Mime, $"{Path.GetFileNameWithoutExtension(file.FileName)}{documentDownload.Extension}");
            }
            catch (Exception e)
            {
                //log
                return BadRequest(e.ToString());
            }
            // TODO: catch different exceptions based on the nature of the error
        }

        [HttpGet]
        [Route("UrlDownload")]
        public async Task<IActionResult> UrlDownload([FromQuery] string url)
        {
            try
            {
                var documentDownloadModel = await documentService.DownloadFromUrlAsync(url);
                return File(documentDownloadModel.Stream, documentDownloadModel.Mime ?? "application/octet-stream", "urlDownload");
            }
            catch (Exception e)
            {
                // log
                return BadRequest();
            }
        }
    }
}
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
        private readonly IWebHostEnvironment env;
        
        private readonly IDocumentService documentService;

        public DocumentController(
            ILogger<DocumentController> logger,
            IDocumentService documentService, 
            IWebHostEnvironment env)
        {
            this.logger = logger;
            this.documentService = documentService;
            this.env = env;
        }

        [HttpPost(Name = "Convert")]
        // TODO: add swagger, allow client generation for "safe" types
        public async Task<IActionResult> Convert(IFormFile file, FileType sourceType, FileType targetType)
        {
            logger.LogInformation($"{nameof(Convert)}({file.FileName})");
            
            try
            {
                using var sr = new StreamReader(file.OpenReadStream());
                var content = await sr.ReadToEndAsync();
                var documentDownload = documentService.ConvertDocument(content, sourceType, targetType);
                return File(documentDownload.Stream, documentDownload.Mime, $"{Path.GetFileNameWithoutExtension(file.FileName)}{documentDownload.Extension}");
            }
            catch (Exception e) // TODO: catch different exceptions based on the nature of the error
            {
                //log
                return BadRequest(e.ToString());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(IFormFile file, string path)
        {
            // validate in document service, throw exception, convert to result in exception filter

            // maybe other checks such as file size
            using var sr = new StreamReader(file.OpenReadStream());
            var content = await sr.ReadToEndAsync();
            try
            {
                path = $"{env.WebRootPath}/{path}";
                documentService.SaveDocumentAsync(file.ContentType, content, path);
            }
            catch (Exception e)
            {
                // log
                return Problem(e.ToString());
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult Get(string path)
        {
            try
            {
                var documentDownload = documentService.GetDocument(path);
                return File(documentDownload.Stream, documentDownload.Mime, documentDownload.Extension);
            }
            catch (Exception e)
            {
                // log
                return NotFound();
            }
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
                return BadRequest(e.ToString());
            }
        }
    }
}
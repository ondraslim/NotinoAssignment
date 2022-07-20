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
                var stream = documentService.ConvertDocument(content, sourceType, targetType);
                return new FileStreamResult(stream, Path.GetExtension(file.FileName));
            }
            catch (Exception e)
            {
                //log
                return BadRequest();
            }
            // TODO: catch different exceptions based on the nature of the error
        }
    }
}
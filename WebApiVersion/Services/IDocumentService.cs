using WebApiVersion.Models;

namespace WebApiVersion.Services;

public interface IDocumentService
{
    DocumentDownloadModel ConvertDocument(string content, FileType targetType, FileType fileType);
    void SaveDocumentAsync(string filename, string content, string path);
    DocumentDownloadModel GetDocument(string path);
    Task<DocumentDownloadModel> DownloadFromUrlAsync(string url);
}
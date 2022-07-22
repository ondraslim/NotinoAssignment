using WebApiVersion.Models;

namespace WebApiVersion.Services;

public interface IDocumentService
{
    DocumentDownloadModel ConvertDocument(string content, FileType targetType, FileType fileType);
    void SaveDocument(string content, FileType type, string path);
    FileStream GetDocument(FileType type, string path);
    Task<DocumentDownloadModel> DownloadFromUrlAsync(string url);
}
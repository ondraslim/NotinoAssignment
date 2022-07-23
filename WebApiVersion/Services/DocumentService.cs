using WebApiVersion.Models;
using WebApiVersion.Services.Serializers.Interfaces;

namespace WebApiVersion.Services;

public class DocumentService : IDocumentService
{
    private readonly IHttpClientFactory httpClientFactory;

    private readonly IXmlDocumentSerializer xmlDocumentSerializer;
    private readonly IJsonDocumentSerializer jsonDocumentSerializer;

    public DocumentService(
        IXmlDocumentSerializer xmlDocumentSerializer,
        IJsonDocumentSerializer jsonDocumentSerializer,
        IHttpClientFactory httpClientFactory)
    {
        this.xmlDocumentSerializer = xmlDocumentSerializer;
        this.jsonDocumentSerializer = jsonDocumentSerializer;
        this.httpClientFactory = httpClientFactory;
    }

    public DocumentDownloadModel ConvertDocument(string content, FileType sourceType, FileType targetType)
    {
        ValidateDocumentConvertType(sourceType, targetType);
        var deserializer = GetSerializerForType(sourceType);
        var document = deserializer.DeserializeDocument(content);

        var serializer = GetSerializerForType(targetType);
        var stream = serializer.SerializeDocument(document);

        var extension = GetExtensionForFileType(targetType);
        var mime = ContentTypeMappingHelper.GetMimeTypeForFileExtension(extension);

        return new DocumentDownloadModel(stream, mime, extension);
    }

    private static void ValidateDocumentConvertType(FileType sourceType, FileType targetType)
    {
        if (sourceType is FileType.Unknown || targetType is FileType.Unknown)
        {
            throw new ArgumentException();
        }

        if (sourceType == targetType)
        {
            throw new ArgumentException();
        }
    }

    public void SaveDocumentAsync(string filename, string content, string path)
    {
        if (!IsAllowedPath(path))
        {
            throw new ArgumentException();
        }

        path = Path.Combine(path, filename);

        using var fs = File.CreateText(path);
        fs.Write(content);
    }

    private static bool IsAllowedPath(string path) => File.Exists(path);

    public DocumentDownloadModel GetDocument(string path)
    {
        if (!IsAllowedPath(path))
        {
            throw new ArgumentException();
        }

        var stream = File.OpenRead(path);
        var mime = ContentTypeMappingHelper.GetMimeTypeForFileExtension(Path.GetExtension(path));
        var extension = Path.GetExtension(path);

        return new DocumentDownloadModel(stream, mime, extension);
    }

    public async Task<DocumentDownloadModel> DownloadFromUrlAsync(string url)
    {
        var httpClient = httpClientFactory.CreateClient();

        var response = await httpClient.GetAsync(url);
        var stream = await response.Content.ReadAsStreamAsync();

        var mime = response.Content.Headers.ContentType?.MediaType;
        var extension = ContentTypeMappingHelper.GetFileExtensionForMime(mime);

        return new DocumentDownloadModel(stream, mime, extension);
    }


    private IDocumentSerializer GetSerializerForType(FileType sourceType)
        => sourceType switch
        {
            FileType.Json => jsonDocumentSerializer,
            FileType.Xml => xmlDocumentSerializer,
            FileType.Unknown => throw new ArgumentException(),
            _ => throw new ArgumentOutOfRangeException(nameof(sourceType), sourceType, null)
        };
    
    private static string GetExtensionForFileType(FileType sourceType)
        => sourceType switch
        {
            FileType.Json => ".json",
            FileType.Xml => ".xml",
            FileType.Unknown => throw new ArgumentException(),
            _ => throw new ArgumentOutOfRangeException(nameof(sourceType), sourceType, null)
        };
}

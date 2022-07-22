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
        var deserializer = GetSerializerForType(sourceType);
        var document = deserializer.DeserializeDocument(content);

        var serializer = GetSerializerForType(targetType);
        var stream = serializer.SerializeDocument(document);

        var extension = GetExtensionForFileType(targetType);
        var mime = ContentTypeMappingHelper.GetMimeTypeForFileExtension(extension);

        return new DocumentDownloadModel(stream, mime, extension);
    }

    public void SaveDocument(string content, FileType type, string path)
    {

    }

    public FileStream GetDocument(FileType type, string path)
    {
        throw new NotImplementedException();
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

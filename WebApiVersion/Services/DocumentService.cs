using WebApiVersion.Models;
using WebApiVersion.Services.Serializers.Interfaces;

namespace WebApiVersion.Services;

public class DocumentService : IDocumentService
{
    private readonly IXmlDocumentSerializer xmlDocumentSerializer;
    private readonly IJsonDocumentSerializer jsonDocumentSerializer;

    public DocumentService(
        IXmlDocumentSerializer xmlDocumentSerializer,
        IJsonDocumentSerializer jsonDocumentSerializer)
    {
        this.xmlDocumentSerializer = xmlDocumentSerializer;
        this.jsonDocumentSerializer = jsonDocumentSerializer;
    }

    public Stream ConvertDocument(string content, FileType sourceType, FileType targetType)
    {
        var deserializer = GetSerializerForType(sourceType);
        var document = deserializer.DeserializeDocument(content);

        var serializer = GetSerializerForType(targetType);
        return serializer.SerializeDocument(document);
    }

    public void SaveDocument(string content, FileType type, string path)
    {

    }

    public FileStream GetDocument(FileType type, string path)
    {
        throw new NotImplementedException();
    }



    private IDocumentSerializer GetSerializerForType(FileType sourceType)
        => sourceType switch
        {
            FileType.Json => jsonDocumentSerializer,
            FileType.Xml => xmlDocumentSerializer,
            FileType.Unknown => throw new ArgumentException(),
            _ => throw new ArgumentOutOfRangeException(nameof(sourceType), sourceType, null)
        };
}

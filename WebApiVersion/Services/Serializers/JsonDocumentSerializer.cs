using System.Text.Json;
using WebApiVersion.Models;
using WebApiVersion.Services.Serializers.Interfaces;

namespace WebApiVersion.Services.Serializers;

public class JsonDocumentSerializer : IJsonDocumentSerializer
{
    public Stream SerializeDocument(DocumentModel documentModel)
    {
        var stream = new MemoryStream();
        JsonSerializer.Serialize(stream, documentModel);
        stream.Seek(0, SeekOrigin.Begin);

        return stream;
    }

    public DocumentModel DeserializeDocument(string content) 
        => JsonSerializer.Deserialize<DocumentModel>(content);
}
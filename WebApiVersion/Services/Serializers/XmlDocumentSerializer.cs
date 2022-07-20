using System.Xml.Serialization;
using WebApiVersion.Models;
using WebApiVersion.Services.Serializers.Interfaces;

namespace WebApiVersion.Services.Serializers;

public class XmlDocumentSerializer : IXmlDocumentSerializer
{
    private readonly XmlSerializer xmlSerializer = new(typeof(DocumentModel));

    public Stream SerializeDocument(DocumentModel documentModel)
    {
        var stream = new MemoryStream();
        xmlSerializer.Serialize(stream, documentModel);
        stream.Seek(0, SeekOrigin.Begin);

        return stream;
    }

    public DocumentModel DeserializeDocument(string content)
    {
        using var stream = StringToStream(content);
        var documentModel = xmlSerializer.Deserialize(stream) as DocumentModel;

        return documentModel;
    }

    private static Stream StringToStream(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;

        return stream;
    }
}
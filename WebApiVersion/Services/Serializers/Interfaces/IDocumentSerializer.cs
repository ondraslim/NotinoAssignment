using WebApiVersion.Models;

namespace WebApiVersion.Services.Serializers.Interfaces;

public interface IDocumentSerializer
{
    Stream SerializeDocument(DocumentModel documentModel);
    DocumentModel DeserializeDocument(string content);
}
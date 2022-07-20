using FluentAssertions;
using WebApiVersion.Models;
using WebApiVersion.Services;
using WebApiVersion.Services.Serializers;

namespace WebApiVersion.Tests;

public class DocumentServiceTests
{
    [Test]
    [TestCase(FileType.Unknown, FileType.Xml)]
    [TestCase(FileType.Xml, FileType.Unknown)]
    [TestCase(FileType.Xml, FileType.Xml)]
    [TestCase(FileType.Json, FileType.Json)]
    public void Convert_CalledWithInvalidFileTypeCombination_ThrowsException(FileType sourceType, FileType targetType)
    {
        var documentService = new DocumentService(new XmlDocumentSerializer(), new JsonDocumentSerializer());

        Action act = () => documentService.ConvertDocument("", sourceType, targetType);

        act.Should().Throw<Exception>();
    }

    [Test]
    [TestCase("title", "text")]
    [TestCase("title", "")]
    [TestCase("", "text")]
    [TestCase("", "")]
    public void Convert_JsonToXml_ConvertsCorrectly(string title, string text)
    {
        var documentService = new DocumentService(new XmlDocumentSerializer(), new JsonDocumentSerializer());

        var stream = documentService.ConvertDocument(TestHelpers.BuildDocumentModelJson(title, text), FileType.Json, FileType.Xml);
        using var streamReader = new StreamReader(stream);
        var serialized = streamReader.ReadToEnd();

        var expected = TestHelpers.BuildDocumentModelXml(title, text);

        serialized.Should().BeEquivalentTo(expected);
    }

    [Test]
    [TestCase("title", "text")]
    [TestCase("title", "")]
    [TestCase("", "text")]
    [TestCase("", "")]
    public void Convert_XmlToJson_ConvertsCorrectly(string title, string text)
    {
        var documentService = new DocumentService(new XmlDocumentSerializer(), new JsonDocumentSerializer());

        var stream = documentService.ConvertDocument(TestHelpers.BuildDocumentModelXml(title, text), FileType.Xml, FileType.Json);
        using var streamReader = new StreamReader(stream);
        var serialized = streamReader.ReadToEnd();

        var expected = TestHelpers.BuildDocumentModelJson(title, text);

        serialized.Should().BeEquivalentTo(expected);
    }
}
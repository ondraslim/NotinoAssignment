using FluentAssertions;
using WebApiVersion.Models;
using WebApiVersion.Services.Serializers;
using WebApiVersion.Tests.Utils;

namespace WebApiVersion.Tests;

public class DocumentSerializerTests
{
    [Test]
    [TestCase("title", "text")]
    [TestCase("title", "")]
    [TestCase("", "text")]
    [TestCase("", "")]
    public void Json_SerializeTests(string title, string text)
    {
        var serializer = new JsonDocumentSerializer();
        var document = new DocumentModel { Title = title, Text = text };

        var stream = serializer.SerializeDocument(document);
        using var streamReader = new StreamReader(stream);
        var serialized = streamReader.ReadToEnd();
        
        var expected = TestHelpers.BuildDocumentModelJson(title, text);
        
        serialized.Should().BeEquivalentTo(expected);
    }

    [Test]
    [TestCase("title", "text")]
    [TestCase("title", "")]
    [TestCase("", "text")]
    [TestCase("", "")]
    public void Json_DeserializeTests(string title, string text)
    {
        var serializer = new JsonDocumentSerializer();
        var content = TestHelpers.BuildDocumentModelJson(title, text);

        var document = serializer.DeserializeDocument(content);

        document.Should().BeEquivalentTo(new DocumentModel{ Title = title, Text = text});
    }

    [Test]
    [TestCase("title", "text")]
    [TestCase("title", "")]
    [TestCase("", "text")]
    [TestCase("", "")]
    public void Xml_SerializeTests(string title, string text)
    {
        var serializer = new XmlDocumentSerializer();
        var document = new DocumentModel { Title = title, Text = text };

        var stream = serializer.SerializeDocument(document);
        using var streamReader = new StreamReader(stream);
        var serialized = streamReader.ReadToEnd();

        var expectedXml = TestHelpers.BuildDocumentModelXml(title, text);

        serialized.Should().BeEquivalentTo(expectedXml);
    }

    [Test]
    [TestCase("title", "text")]
    [TestCase("title", "")]
    [TestCase("", "text")]
    [TestCase("", "")]
    public void Xml_DeserializeTests(string title, string text)
    {
        var serializer = new XmlDocumentSerializer();
        
        var content = TestHelpers.BuildDocumentModelXml(title, text);
        var document = serializer.DeserializeDocument(content);
        
        var expectedDocument = new DocumentModel { Title = title, Text = text };

        document.Should().BeEquivalentTo(expectedDocument);
    }
}
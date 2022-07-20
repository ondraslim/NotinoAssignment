using System.Text;

namespace WebApiVersion.Tests;

public static class TestHelpers
{
    public static string BuildDocumentModelXml(string title, string text)
    {
        var sb = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?><DocumentModel xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
        if (string.IsNullOrEmpty(title))
        {
            sb.Append("<Title />");
        }
        else
        {
            sb.Append($"<Title>{title}</Title>");
        }

        if (string.IsNullOrEmpty(text))
        {
            sb.Append("<Text />");
        }
        else
        {
            sb.Append($"<Text>{text}</Text>");
        }

        sb.Append("</DocumentModel>");
        return sb.ToString();
    }


    public static string BuildDocumentModelJson(string title, string text)
        => $"{{\"Title\":\"{title}\",\"Text\":\"{text}\"}}";
}
using Microsoft.AspNetCore.StaticFiles;

namespace WebApiVersion;

public static class ContentTypeMappingHelper
{
    private static readonly FileExtensionContentTypeProvider Provider;

    static ContentTypeMappingHelper()
    {
        Provider = new FileExtensionContentTypeProvider();
    }

    public static string GetMimeTypeForFileExtension(string filePath)
    {
        const string defaultContentType = "application/octet-stream";
        if (!Provider.TryGetContentType(filePath, out var contentType))
        {
            contentType = defaultContentType;
        }

        return contentType;
    }
    public static string GetFileExtensionForMime(string mime)
    {
        const string defaultExtension = ".txt";
        var extension = Provider.Mappings.FirstOrDefault(m => m.Value == mime).Key;
        if (string.IsNullOrEmpty(extension))
        {
            extension = defaultExtension;
        }

        return extension;
    }
}
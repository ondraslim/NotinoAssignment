namespace WebApiVersion.Models;

public record DocumentDownloadModel(Stream Stream, string? Mime, string Extension);
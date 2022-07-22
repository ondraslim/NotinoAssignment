namespace WebApiVersion.Tests.Utils;

// taken from: https://stackoverflow.com/a/63155321
public sealed class DefaultHttpClientFactory : IHttpClientFactory, IDisposable
{
    private readonly Lazy<HttpMessageHandler> handlerLazy = new(() => new HttpClientHandler());

    public HttpClient CreateClient() => new(handlerLazy.Value, disposeHandler: false);
    public HttpClient CreateClient(string name) => new(handlerLazy.Value, disposeHandler: false);

    public void Dispose()
    {
        if (handlerLazy.IsValueCreated)
        {
            handlerLazy.Value.Dispose();
        }
    }
}
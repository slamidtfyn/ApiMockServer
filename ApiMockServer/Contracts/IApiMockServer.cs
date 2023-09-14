namespace ApiMockServer;

public interface IApiMockServer
{
    Task<HttpClient> Start(CancellationToken cancellationToken = default);
}
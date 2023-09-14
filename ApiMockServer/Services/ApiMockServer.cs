using ApiMockServer.Contracts;

namespace ApiMockServer;

public class ApiMockServer : IApiMockServer, IApiMockServerConfig
{
    const string Host = "http://localhost";
    private ApiMockServerConfigModel _config = new("5000", new List<ApiMockServerConfigModelActions>());
    public static IApiMockServerConfig Instance { get; } = new ApiMockServer();

    private ApiMockServer()
    {
    }

    public async Task<HttpClient> Start(CancellationToken cancellationToken = default)
    {
        new Thread(TreadProc)
        {
            IsBackground = true
        }.Start(_config);

        await WaitForServerReady(_config.Port, cancellationToken);


        return new HttpClient() { BaseAddress = new Uri($"{Host}:{_config.Port}") };
    }

    private static async Task WaitForServerReady(string port, CancellationToken cancellationToken)
    {
        using var httpClient = new HttpClient()
        {
            BaseAddress = new Uri($"{Host}:{port}"), 
            Timeout = TimeSpan.FromMilliseconds(1000)
        };
        var ready = false;
        var retryCount = 2;
        while (!ready)
        {
            Thread.Sleep(retryCount);
            try
            {
                var result = await httpClient.GetAsync("/hc", cancellationToken)
                    .ConfigureAwait(false);
                ready = result.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                ready = false;
            }

            retryCount *= 2;
        }
    }

    private static void TreadProc(object? config)
    {
        var configModel = config as ApiMockServerConfigModel ??
                          throw new NullReferenceException("config is invalid");
        var builder = WebApplication.CreateBuilder();
        builder.Logging.ClearProviders();
        var app = builder.Build();
        app.MapGet("/hc", () => "It's alive!");
        foreach (var (path, response) in configModel.Actions)
        {
            app.MapGet(path, response);
        }

        app.Run($"{Host}:{configModel.Port}");
    }

    public IApiMockServer Config(ApiMockServerConfigModel? config)
    {
        _config = config ?? _config;
        return this;
    }
}
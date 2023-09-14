namespace ApiConsumerService;

public class Calculator
{
    private readonly IApiConsumerClient _apiConsumerService;

    public Calculator(IApiConsumerClient apiConsumerService)
    {
        _apiConsumerService = apiConsumerService;
    }

    public async Task<int> Add(int a, int b) => await _apiConsumerService.AddAsync(a, b);
}

public class ApiConsumerClient : IApiConsumerClient
{
    private readonly HttpClient _httpClient;

    public ApiConsumerClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        const string host = "http://localhost:5000";
        _httpClient.BaseAddress = new Uri(host);
    }

    public async Task<int> AddAsync(int a, int b)
    {
        var result = await _httpClient.GetStringAsync($"/api/add?a={a}&b={b}");
        return int.Parse(result);
    }
}

public interface IApiConsumerClient
{
    Task<int> AddAsync(int a, int b);
}
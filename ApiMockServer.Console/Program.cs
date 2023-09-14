using ApiMockServer;

var apiClient = await ApiMockServer.ApiMockServer
    .Instance.Config(new ApiMockServerConfigModel("5000",new []
    {
        new ApiMockServerConfigModelActions("/", () => "Hello World from config")
    })).Start();

var result = await apiClient.GetStringAsync("/");
Console.WriteLine(result);



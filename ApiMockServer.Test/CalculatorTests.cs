using System.Threading.Tasks;
using ApiConsumerService;
using Xunit;


namespace ApiMockServer.Test;

public class CalculatorTests
{
    private readonly IApiMockServer _mockApi = ApiMockServer.Instance.Config(new ApiMockServerConfigModel("5000", new[]
    {
        new ApiMockServerConfigModelActions("/api/add", () => "3")
    }));

    [Fact]
    public async Task CalculatorTest()
    {
        var sut = new Calculator(new ApiConsumerClient(await _mockApi.Start()));
        var result = await sut.Add(1, 2);
        Assert.Equal(3, result);
    }
}
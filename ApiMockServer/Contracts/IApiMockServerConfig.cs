namespace ApiMockServer.Contracts;

public interface IApiMockServerConfig
{
    IApiMockServer Config(ApiMockServerConfigModel config);
}
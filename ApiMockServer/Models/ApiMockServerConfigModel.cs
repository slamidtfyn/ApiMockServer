namespace ApiMockServer;

public record ApiMockServerConfigModel(string Port, IEnumerable<ApiMockServerConfigModelActions> Actions);
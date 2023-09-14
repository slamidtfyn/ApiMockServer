namespace ApiMockServer;

public record ApiMockServerConfigModelActions(string Path, Func<object> Handler);
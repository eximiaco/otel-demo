namespace OtelDemo.Inscricoes.IntegrationTests.TestInfrastructure.DockerHelper;

public interface IDockerUtilitiesLogger
{
    void Debug(string message);
    void Information(string message);
    void Error(string message, Exception exception);
}

/// <summary>
/// TODO: Adicionar logger dentro das estruturas internas do Docker.
/// </summary>
public class DockerUtilitiesLogger : IDockerUtilitiesLogger
{
    public static IDockerUtilitiesLogger Dump = new DockerUtilitiesLogger(null);

    private readonly Action<string> _logger;
    public DockerUtilitiesLogger(Action<string> logger)
    {
        _logger = logger;
    }

    public void Debug(string message)
    {
        _logger?.Invoke(message);
    }

    public void Information(string message)
    {
        _logger?.Invoke(message);
    }

    public void Error(string message, Exception exception)
    {
        _logger?.Invoke(message);
    }
}

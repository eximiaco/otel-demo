namespace OtelDemo.Inscricoes.IntegrationTests.TestInfrastructure.DockerHelper;

public abstract class DockerRegistry
{
    protected readonly List<string> RegisteredContainers = new List<string>();
    protected readonly DockerEngine DockerEngine;

    public DockerRegistry(DockerEngine dockerEngine)
    {
        DockerEngine = dockerEngine;
    }

    public abstract Task DownloadImageAsync();
    public abstract Task InstallContainerAsync();
    public abstract Task HealthCheckContainerAsync();

    public async Task CleanAsync()
    {
        foreach (var containerId in RegisteredContainers)
        {
            await DockerEngine.RemoveContainerAsync(containerId);
        }
    }

    protected void StoreContainerId(string containerId)
    {
        if (string.IsNullOrEmpty(containerId))
            throw new ArgumentNullException(nameof(containerId));

        RegisteredContainers.Add(containerId);
    }
}

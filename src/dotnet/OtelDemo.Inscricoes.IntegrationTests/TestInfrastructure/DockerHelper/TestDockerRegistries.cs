namespace OtelDemo.Inscricoes.IntegrationTests.TestInfrastructure.DockerHelper;

public class TestDockerRegistries
{
    private readonly List<DockerRegistry> _registries = new List<DockerRegistry>();

    public void AddRegistry(DockerRegistry dockerRegistry)
    {
        _registries.Add(dockerRegistry);
    }

    public async Task RunAsync()
    {
        foreach (var register in _registries)
        {
            await register.DownloadImageAsync();
            await register.InstallContainerAsync();
            await register.HealthCheckContainerAsync();
        }
    }

    public async Task CleanAsync()
    {
        foreach (var register in _registries)
        {
            await register.CleanAsync();
        }
    }
}

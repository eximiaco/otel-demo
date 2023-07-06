using Docker.DotNet;
using OtelDemo.Inscricoes.IntegrationTests.TestInfrastructure.DockerHelper;

namespace OtelDemo.Inscricoes.IntegrationTests.TestInfrastructure.PostgresIntegrationUtilities;

/// <summary>
/// Responsável por inicializar um container do Docker para o postgres do banco Plataform-Integration
/// </summary>
public class PostgresDockerCollectionFixture : IAsyncLifetime
{
    private readonly IDockerClient _dockerClient = DockerClientBuilder.Build();

    private readonly PostgresDockerSettings _settings;
    private readonly TestDockerRegistries _dockerRegistries;

    public PostgresDockerCollectionFixture()
    {
        _dockerRegistries = new TestDockerRegistries();
        _settings = StringConnectionHolder.GetInstance();
        _dockerRegistries.RegisterPostgres(_dockerClient, _settings);
    }

    public string GetConnectionString()
    {
        return _settings.GetConnectionString();
    }

    public async Task InitializeAsync()
    {
        await _dockerRegistries.RunAsync();
    }

    public Task DisposeAsync()
    {
        return _dockerRegistries.CleanAsync();
    }
}

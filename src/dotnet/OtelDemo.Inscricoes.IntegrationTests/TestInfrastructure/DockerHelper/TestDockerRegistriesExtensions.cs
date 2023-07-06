using Docker.DotNet;

namespace OtelDemo.Inscricoes.IntegrationTests.TestInfrastructure.DockerHelper;

public static class TestDockerRegistriesExtensions
{
    /// <summary>
    /// Adiciona o Registrador para Postgres
    /// </summary>
    /// <param name="testDockerRegistries"></param>
    /// <param name="dockerClient"></param>
    /// <param name="settings"></param>
    public static void RegisterPostgres(this TestDockerRegistries testDockerRegistries, IDockerClient dockerClient, PostgresDockerSettings settings)
    {
        var dockerEngine = new DockerEngine(dockerClient);
        testDockerRegistries.AddRegistry(new PostgresRegistry(dockerEngine, settings));
    }
}

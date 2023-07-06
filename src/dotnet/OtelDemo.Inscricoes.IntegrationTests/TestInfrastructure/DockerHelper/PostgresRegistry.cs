using Docker.DotNet.Models;
using Npgsql;

namespace OtelDemo.Inscricoes.IntegrationTests.TestInfrastructure.DockerHelper;

public class PostgresRegistry : DockerRegistry
{
    private readonly PostgresDockerSettings _settings;
    private readonly DockerImageInfo _dockerImageInfo;

    public PostgresRegistry(DockerEngine dockerEngine, PostgresDockerSettings settings) : base(dockerEngine)
    {
        _settings = settings;
        _dockerImageInfo = DockerImageInfo.New(_settings);
    }

    public override async Task HealthCheckContainerAsync()
    {
        await WaitUntilDatabaseAvailableAsync(
            _settings.TotalTimeToWaitUntilDatabaseStartedInSeconds,
            _settings.GetConnectionString()
        );
    }

    public override async Task DownloadImageAsync()
    {
        await DockerEngine.DownloadImageAsync(_dockerImageInfo);
    }

    public override async Task InstallContainerAsync()
    {
        await DockerEngine.RemoveContainersByPrefixAsync(_settings.DockerContainerPrefix);
        await DockerEngine.CreateImageAsync(_dockerImageInfo);
        var containerId = await DockerEngine.EnsureCreateAndStartContainer(SqlParameters(_dockerImageInfo));
        StoreContainerId(containerId);
    }

    private CreateContainerParameters SqlParameters(DockerImageInfo dockerImageInfo)
    {
        return new CreateContainerParameters
        {
            Name = _settings.DockerContainerPrefix + Guid.NewGuid(),
            Image = dockerImageInfo.Image,
            Env = new List<string>
            {
               "POSTGRES_PASSWORD=postgres",
               "POSTGRES_USER=postgres"
            },
            HostConfig = new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    {
                        "5432/tcp",
                        new PortBinding[]
                        {
                            new PortBinding
                            {
                                HostPort = _settings.DatabasePort
                            }
                        }
                    }
                }
            }
        };
    }
    private static async Task WaitUntilDatabaseAvailableAsync(int maxWaitTimeInSeconds, string connectionString)
    {
        var start = DateTime.UtcNow;
        var connectionEstablished = false;

        while (!connectionEstablished && start.AddSeconds(maxWaitTimeInSeconds) > DateTime.UtcNow)
        {
            try
            {
                await using var sqlConnection = new NpgsqlConnection(connectionString);
                await sqlConnection.OpenAsync();
                connectionEstablished = true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                // If opening the SQL connection fails, SQL Server is not ready yet
                await Task.Delay(500);
            }
        }

        if (!connectionEstablished)
        {
            throw new Exception("Connection to the SQL docker database could not be established within 60 seconds.");
        }

        // APÓS conectar ao banco é necessário aguardar uma sequência de execuções dentro do banco. Aguardando 5 segundos.
        await Task.Delay(5000);
    }
}

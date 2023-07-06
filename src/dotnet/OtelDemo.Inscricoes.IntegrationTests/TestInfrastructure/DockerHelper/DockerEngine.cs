using Docker.DotNet;
using Docker.DotNet.Models;

namespace OtelDemo.Inscricoes.IntegrationTests.TestInfrastructure.DockerHelper;

/// <summary>
/// Layer de interação com o Docker.
/// </summary>
public class DockerEngine
{
    private readonly IDockerClient _dockerClient;
    private readonly AuthConfig? _authConfig;
    public DockerEngine(IDockerClient dockerClient)
    {
        _dockerClient = dockerClient ?? throw new ArgumentNullException(nameof(dockerClient));
        
        //_authConfig = null;
        _authConfig = new AuthConfig()
        {
            Username = "gabrieleximia",
            Password = "P8pa8A6uNYku6r",
            //Password = "dckr_pat_z-qSvfmYPiiBEyD1AQc8w8siLTg",
            Email = "gabriel@eximia.co",
            ServerAddress = ""
        };
    }

    /// <summary>
    /// Efetua download de uma determinada imagem baseado em seu nome e tag.
    /// </summary>
    /// <param name="dockerImageInfo">Armazena Tag e Name da imagem</param>
    /// <returns>Success</returns>
    public async Task DownloadImageAsync(DockerImageInfo dockerImageInfo)
    {
        await _dockerClient.Images
            .CreateImageAsync(new ImagesCreateParameters
            {
                FromImage = dockerImageInfo.Image
            },
            _authConfig,
            new Progress<JSONMessage>()
        );
    }

    /// <summary>
    /// Cria a imagem de uma determinada imagem baseado em seu nome e tag.
    /// </summary>
    /// <param name="dockerImageInfo">Armazena Tag e Name da imagem</param>
    /// <returns></returns>
    public async Task CreateImageAsync(DockerImageInfo dockerImageInfo)
    {
        await _dockerClient.Images
            .CreateImageAsync(new ImagesCreateParameters
            {
                FromImage = dockerImageInfo.Image
            },
            _authConfig,
            new Progress<JSONMessage>()
        );
    }

    /// <summary>
    /// Remove todos os containers baseado em um prefixo. Por exemplo:
    ///     prefix-container-1      REMOVED
    ///     prefix-container-2      REMOVED
    ///     another-container-1     NOT REMOVED
    /// </summary>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public async Task RemoveContainersByPrefixAsync(string prefix)
    {
        var runningContainers = await _dockerClient
            .Containers
            .ListContainersAsync(new ContainersListParameters());

        var testContainers = runningContainers.Where(cont => cont.Names.Any(n => n.Contains(prefix)));
        foreach (var currentContainer in testContainers)
        {
            // Stopping all test containers that are older than one hour, they likely failed to cleanup
            if (currentContainer.Created < DateTime.UtcNow.AddHours(-1))
            {
                try
                {
                    await RemoveContainerAsync(currentContainer.ID);
                }
                catch (Exception)
                {
                    // Ignoring failures to stop running containers
                    // _logger.Error(e.Message, e);
                }
            }
        }
    }

    /// <summary>
    /// Remove o container que pertence ao <paramref name="containerId"/>.
    /// </summary>
    /// <param name="containerId"></param>
    /// <returns></returns>
    public async Task RemoveContainerAsync(string containerId)
    {
        await _dockerClient.Containers
           .StopContainerAsync(containerId, new ContainerStopParameters());

        await _dockerClient.Containers
            .RemoveContainerAsync(containerId, new ContainerRemoveParameters());
    }

    /// <summary>
    /// Garante a criação e inicialização de um determinado container.
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public async Task<string> EnsureCreateAndStartContainer(CreateContainerParameters parameters)
    {
        var id = await CreateContainerAsync(parameters);
        await StartContainerAsync(id);
        return id;
    }

    /// <summary>
    /// Inicializa o container com <paramref name="containerId"/>
    /// </summary>
    /// <param name="containerId"></param>
    /// <returns></returns>
    public async Task StartContainerAsync(string containerId)
    {
        await _dockerClient
            .Containers
            .StartContainerAsync(containerId, new ContainerStartParameters());
    }

    /// <summary>
    /// Cria o container baseado nos <paramref name="parameters"/>.
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public async Task<string> CreateContainerAsync(CreateContainerParameters parameters)
    {
        var container = await _dockerClient
            .Containers
            .CreateContainerAsync(parameters);

        return container.ID;
    }
}



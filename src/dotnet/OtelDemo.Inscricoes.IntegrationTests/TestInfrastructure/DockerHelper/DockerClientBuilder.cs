using Docker.DotNet;

namespace OtelDemo.Inscricoes.IntegrationTests.TestInfrastructure.DockerHelper;

internal class DockerClientBuilder
{
    private static IDockerClient _dockerClient = null!;

    private const string WindowsUri = "npipe://./pipe/docker_engine";
    private const string UnixUri = "unix:///var/run/docker.sock";

    internal static IDockerClient Build()
    {
        if (_dockerClient != null)
            return _dockerClient;

        var dockerUri = SelectUriBasedOnEnv();
        _dockerClient = new DockerClientConfiguration(dockerUri)
            .CreateClient();

        return _dockerClient;
    }

    private static Uri SelectUriBasedOnEnv()
    {
        return IsRunningOnWindows() ? new Uri(WindowsUri) : new Uri(UnixUri);
    }

    private static bool IsRunningOnWindows()
    {
        return Environment.OSVersion.Platform == PlatformID.Win32NT;
    }
}

namespace OtelDemo.Inscricoes.IntegrationTests.TestInfrastructure.DockerHelper;

public class DockerImageInfo
{
    private DockerImageInfo(string name, string tag)
    {
        Name = name;
        Tag = tag;
    }

    public string Name { get; private set; }

    public string Tag { get; private set; }

    public string Image => $"{Name}:{Tag}";

    public static DockerImageInfo New(string name, string tag)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));

        if (string.IsNullOrEmpty(tag))
            throw new ArgumentNullException(nameof(tag));

        return new DockerImageInfo(name, tag);
    }

    public static DockerImageInfo New(PostgresDockerSettings postgresDockerSettings)
    {
        return new DockerImageInfo(
            postgresDockerSettings.DockerImageName,
            postgresDockerSettings.DockerImageTag
        );
    }
}

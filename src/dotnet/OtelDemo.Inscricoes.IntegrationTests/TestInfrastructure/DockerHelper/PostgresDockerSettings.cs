namespace OtelDemo.Inscricoes.IntegrationTests.TestInfrastructure.DockerHelper;

/// <summary>
/// Configurações para criar um container de Postgres no docker
/// </summary>
public sealed class PostgresDockerSettings
{
    public PostgresDockerSettings(
        string databaseName, 
        string dockerImageName, 
        string dockerImageTag, 
        string dockerContainerPrefix, 
        int? databasePort,
        int waitUntil = 60)
    {
        DatabaseName = databaseName;
        DockerImageName = dockerImageName;
        DockerImageTag = dockerImageTag;
        DockerContainerPrefix = dockerContainerPrefix;
        DatabasePort = databasePort.HasValue ? databasePort.Value.ToString() : TcpPortSelector.GetFreePort().ToString();
        TotalTimeToWaitUntilDatabaseStartedInSeconds = waitUntil;
    }

    /// <summary>
    /// Nome da instância do banco de dados.
    /// </summary>
    public string DatabaseName { get; set; }
    
    /// <summary>
    /// Endereço para a imagem do docker a ser utilizado.
    /// Por exemplo: mcr.microsoft.com/mssql/server
    /// </summary>
    public string DockerImageName { get; set; }

    /// <summary>
    /// Utilizado para determimnar qual é a Tag da imagem do docker que desejamos baixar.
    /// Por exemplo: 2019-latest
    /// </summary>
    public string DockerImageTag { get; set; }

    /// <summary>
    /// Utilizado para facilitar na limpeza de containers durante a execução dos testes.
    /// Por exemplo: MyProjectIntegrationTestsSql
    /// </summary>
    public string DockerContainerPrefix { get; set; }

    /// <summary>
    /// Porta que o banco deve utilizar no container.
    /// Caso null, será selecionada uma porta aleatória durante a criação do container.
    /// </summary>
    public string DatabasePort { get; set; }

    /// <summary>
    /// Define quantidade de tempo a ser aguardando enquanto o banco de dados inicializa.
    /// </summary>
    public int TotalTimeToWaitUntilDatabaseStartedInSeconds { get; set; }

    public string GetConnectionString()
    {
        return $"Username=postgres;Password=postgres;Host=localhost;Port={DatabasePort};Database={DatabaseName};Pooling=true;Multiplexing=false;";
    }
}

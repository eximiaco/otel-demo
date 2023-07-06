using OtelDemo.Inscricoes.IntegrationTests.TestInfrastructure.DockerHelper;

namespace OtelDemo.Inscricoes.IntegrationTests.TestInfrastructure.PostgresIntegrationUtilities;

/// <summary>
/// Mantém Settings necessárias para construir um container com o banco de dados da docway em SQL Server
/// </summary>
public static class StringConnectionHolder
{
    /// <summary>
    /// Informações padrões para construir e logar no banco de dados durante o teste de integração.
    /// TODO: Tornar variável de ambiente - pode ser preenchido no pipeline de Publicação.
    /// </summary>
    private static PostgresDockerSettings _settings = new PostgresDockerSettings(
        "postgres",
        "gabrieleximia/otel_db",
        "latest",
        "integration-tests-db",
        null,
        120
    );

    /// <summary>
    /// Retorna a instância de configuração para Instãncia de SqlServer baseado na imagem docway.azurecr.io/mssql
    /// </summary>
    /// <returns></returns>
    internal static PostgresDockerSettings GetInstance() => _settings;
}

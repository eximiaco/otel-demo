using Xunit.Extensions.Ordering;

namespace OtelDemo.Inscricoes.IntegrationTests.TestInfrastructure.PostgresIntegrationUtilities;

/// <summary>
/// Classe base para testes de Integração.
/// O IAssemblyFixture determina que o escopo da classe de teste será executado uma vez por projeto de teste. 
/// Ou seja, InitializeAsync e DisposeAsync esta a nível de Suite de testes.
///     -> Assembly Scope <AssemblyFixture>.
///         -> Class Scope <ClassFixture>.
///             -> Test Scope <Contructor> and <Dispose>.
/// </summary>
public abstract class PostgresIntegrationTestBase : IAsyncLifetime, IAssemblyFixture<PostgresDockerCollectionFixture>
{
    protected readonly PostgresDockerCollectionFixture Fixture;

    protected PostgresIntegrationTestBase(PostgresDockerCollectionFixture fixture)
    {
        Fixture = fixture;
    }

    public string GetConnectionString()
    {
        return Fixture.GetConnectionString();
    }
    
    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}

using Microsoft.Extensions.Caching.Memory;
using Moq;
using OtelDemo.Common.ServiceBus;
using OtelDemo.Inscricoes.InscricoesContext.Infrastructure;
using OtelDemo.Inscricoes.InscricoesContext.Inscricoes;
using OtelDemo.Inscricoes.InscricoesContext.Inscricoes.Comandos;
using OtelDemo.Inscricoes.InscricoesContext.Inscricoes.Eventos;
using OtelDemo.Inscricoes.IntegrationTests.TestInfrastructure.PostgresIntegrationUtilities;
using Shouldly;

namespace OtelDemo.Inscricoes.IntegrationTests.RealizarInscricoes;

public class RealizarInscricoesHandlerTests: PostgresIntegrationTestBase
{
    private SeedDataFixture? _fixture;
    
    public RealizarInscricoesHandlerTests(PostgresDockerCollectionFixture fixture) : base(fixture)
    {
    }
    
    [Fact]
    public async Task Devo_Realizar_Inscricao_Para_Aluno_E_Responsavel_Iguais()
    {
        // Arrange
        var aluno = "11033867004";
        var responsavel = aluno;
        var turma = await _fixture!.SeedTurma(2);
        await _fixture!.SeedAluno(aluno);
        await _fixture!.SeedResponsavel(responsavel);
        var comando = RealizarInscricaoComando.Criar(aluno, responsavel, turma.Id);
        var telemetryFactory = TelemetryMockFactory.Create();
        var serviceBus = new Mock<IServiceBus>();
        serviceBus.Setup(c => c.PublishAsync(new InscricaoRealizadaEvento(It.IsAny<Guid>(), responsavel)));
        var efContext = new InscricoesDbContextFactory(_fixture!.Configuration, serviceBus.Object);
        var efAccessor = new InscricoesDbContextAccessor();
        efAccessor.Register(await efContext.CriarAsync(""));
        var uow = new EfUnitOfWork(efAccessor);
        var sut = new RealizarInscricaoHandler(
            telemetryFactory,
            new InscricoesRepositorio(telemetryFactory, efAccessor),
            uow);

        // Act
        var result = await sut.Executar(comando.Value, new CancellationToken());
        
        // Assert
        result.IsSuccess.ShouldBe(true);
        await _fixture.AssertInsertedInscricaoData(Inscricao.CriarNova(aluno, responsavel, turma).Value);
        //serviceBus.Verify(c=> c.PublishAsync(new InscricaoRealizadaEvento(It.IsAny<Guid>(), responsavel)), Times.Once);
    }
    
    public override Task InitializeAsync()
    {
        _fixture = new SeedDataFixture(Fixture);
        return Task.CompletedTask;
    }

    public override async Task DisposeAsync()
    {
        await _fixture!.Cleanup();
    }
}
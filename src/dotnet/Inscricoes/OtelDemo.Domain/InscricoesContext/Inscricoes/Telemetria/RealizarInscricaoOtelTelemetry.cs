using System.Diagnostics;
using System.Diagnostics.Metrics;
using CSharpFunctionalExtensions;
using OtelDemo.Common.OpenTelemetry;
using OtelDemo.Domain.InscricoesContext.Inscricoes.Comandos;
using OtelDemo.Inscricoes.InscricoesContext.Shared.Telemetria;

namespace OtelDemo.Domain.InscricoesContext.Inscricoes.Telemetria;

public sealed class RealizarInscricaoOtelTelemetry: IRealizarInscricaoTelemetry
{
    private readonly ITelemetryService _telemetryService;
    private readonly OtelMetrics _otelMetrics;
    private readonly OtelVariables _otelVariables;

    public RealizarInscricaoOtelTelemetry(
        ITelemetryFactory telemetryFactory,
        OtelMetrics otelMetrics,
        OtelVariables otelVariables)
    {
        _telemetryService = telemetryFactory.Create($"{nameof(RealizarInscricaoHandler.Executar)}");
        _otelMetrics = otelMetrics;
        _otelVariables = otelVariables;
    }
    
    public void Dispose()
    {
        _telemetryService?.Dispose();
    }

    private static readonly ActivitySource MyActivitySource = new ActivitySource(
        "MyCompany.MyProduct.MyLibrary");
    
    
    
    public void HandleNewOrder()
    {
        var meter = new Meter("ecommerce.metrics.orders");
        var orderCount = meter.CreateCounter<int>("ecommerce.metrics.orders.count", "order");
        
       #region Process new order
       
       #endregion
        
        orderCount.Add(1,
            new KeyValuePair<string, object?>("status", "success"),
            new KeyValuePair<string, object?>("type", "online"));
    }
    
    
    public void NovaInscricaoRecebida(RealizarInscricaoComando comando)
    {
        _telemetryService
            .AddTag(_otelVariables.AlunoId, comando.Aluno)
            .AddTag(_otelVariables.ResponsavelId, comando.Responsavel)
            .AddTag(_otelVariables.TurmaId, comando.Turma);
        _otelMetrics.NovaInscricaoRequisitada(comando.Turma);
    }

    public Result AlunoNaoLocalizado(RealizarInscricaoComando comando)
    {
        _telemetryService.SetError("Aluno não foi localizado", new {});
        _otelMetrics.InscricaoNaoRealizada(comando.Turma);
        return Result.Failure("Aluno inválido");
    }

    public void AlunoLocalizado()
    {
        _telemetryService.AddInformationEvent("Aluno localizado");
    }

    public Result ResponsavelNaoLocalizado(RealizarInscricaoComando comando)
    {
        _telemetryService.SetError("Responsável não foi localizado", new {});
        _otelMetrics.InscricaoNaoRealizada(comando.Turma);
        return Result.Failure("Responsavel inválido");
    }

    public void ResponsavelLocalizado()
    {
        _telemetryService.AddInformationEvent("Responsável localizado");
    }

    public Result TurmaNaoLocalizada(RealizarInscricaoComando comando)
    {
        _telemetryService.SetError("Turma não foi localizada", new {});
        _otelMetrics.InscricaoNaoRealizada(comando.Turma);
        return Result.Failure("Turma inválido");
    }

    public void TurmaLocalizada(Turma turma)
    {
        _telemetryService
            .AddTag(_otelVariables.QuantidadeVagas, turma.Vagas)
            .AddInformationEvent("Turma localizada");
    }

    public Result NaoFoiPossivelCriarInscricao(RealizarInscricaoComando comando, string error)
    {
        _telemetryService.SetError("Falha ao criar inscricao [{error}]", new { error = error });
        _otelMetrics.InscricaoNaoRealizada(comando.Turma);
        return Result.Failure("Falha ao realizar a inscrição");
    }

    public void InscricaoRelizada(Inscricao inscricao)
    {
        _telemetryService
            .AddTag(_otelVariables.InscricaoId, inscricao.Id)
            .SetSucess("Inscrição realizada", new {});
        _otelMetrics.InscricaoRealizada(inscricao.Turma);
    }
}
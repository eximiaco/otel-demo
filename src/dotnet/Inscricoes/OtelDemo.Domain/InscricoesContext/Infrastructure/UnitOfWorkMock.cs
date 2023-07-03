using OtelDemo.Common.OpenTelemetry;
using OtelDemo.Common.UoW;

namespace OtelDemo.Inscricoes.InscricoesContext.Infrastructure;

public class UnitOfWorkMock : IUnitOfWork
{
    private readonly ITelemetryFactory _telemetryFactory;

    public UnitOfWorkMock(ITelemetryFactory telemetryFactory)
    {
        _telemetryFactory = telemetryFactory;
    }
    
    public Task Salvar(CancellationToken cancellationToken)
    {
        var activity = _telemetryFactory.Create($"{nameof(UnitOfWorkMock)}.{nameof(Salvar)}");
        activity.SetSucess("Commit realizado", new{});
        return Task.CompletedTask;
    }
}
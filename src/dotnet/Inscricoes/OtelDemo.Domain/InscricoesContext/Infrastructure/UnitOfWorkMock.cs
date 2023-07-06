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
    
    public Task Commit(CancellationToken cancellationToken)
    {
        var activity = _telemetryFactory.Create($"{nameof(UnitOfWorkMock)}.{nameof(Commit)}");
        activity.SetSucess("Commit realizado", new{});
        return Task.CompletedTask;
    }
}
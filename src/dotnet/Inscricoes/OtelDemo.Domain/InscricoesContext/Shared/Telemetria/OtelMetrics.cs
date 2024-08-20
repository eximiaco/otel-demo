using System.Diagnostics.Metrics;

namespace OtelDemo.Inscricoes.InscricoesContext.Shared.Telemetria;

public class OtelMetrics
{
    public OtelMetrics(string name = "inscricoes.metrics")
    {
        Name = name;
        var meter = new Meter(Name);
        InscricoesCount = meter.CreateUpDownCounter<int>("inscricoes.metrics.total", "inscricao");
        InscricoesErroCount = meter.CreateUpDownCounter<int>("inscricoes.metrics.error", "inscricao");
    }

    public string Name { get; }
    
    private UpDownCounter<int> InscricoesCount { get; }
    private UpDownCounter<int> InscricoesErroCount { get; }
    
    public void InscricaoNaoRealizada(int turma)
    {
        InscricoesErroCount.Add(1,
            new KeyValuePair<string, object?>("turma", turma));
    }

    public void InscricaoRealizada(int turmaId)
    {
        InscricoesCount.Add(1,
            new KeyValuePair<string, object?>("turma", turmaId));
    }
}
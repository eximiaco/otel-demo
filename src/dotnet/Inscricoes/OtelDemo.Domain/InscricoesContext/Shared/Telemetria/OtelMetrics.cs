using System.Diagnostics.Metrics;

namespace OtelDemo.Inscricoes.InscricoesContext.Shared.Telemetria;

public class OtelMetrics
{
    public OtelMetrics(string name = "inscricoes.metrics")
    {
        Name = name;
        var meter = new Meter(Name);
        InscricoesCount = meter.CreateCounter<int>("inscricoes.metrics.total", "inscricao");
    }

    public string Name { get; }
    private Counter<int> InscricoesCount { get; }
    
    public void NovaInscricaoRequisitada(int turma)
    {
        InscricoesCount.Add(1,
            new KeyValuePair<string, object?>("status", "requisitado"),
            new KeyValuePair<string, object?>("turma", turma));
    }

    public void InscricaoNaoRealizada(int turma)
    {
        InscricoesCount.Add(1,
            new KeyValuePair<string, object?>("status", "erro"),
            new KeyValuePair<string, object?>("turma", turma));
    }

    public void InscricaoRealizada(int turmaId)
    {
        InscricoesCount.Add(1,
            new KeyValuePair<string, object?>("status", "sucesso"),
            new KeyValuePair<string, object?>("turma", turmaId));
    }
}
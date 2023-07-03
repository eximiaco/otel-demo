namespace OtelDemo.Inscricoes.FinanceiroContext;

public sealed class TabelaMensalidade
{
    public Guid Id { get; }
    public int TurmaId { get; }
    public decimal Valor { get; }
}
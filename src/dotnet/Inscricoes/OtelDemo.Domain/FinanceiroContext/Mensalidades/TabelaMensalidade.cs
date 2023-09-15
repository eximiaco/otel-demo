namespace OtelDemo.Inscricoes.FinanceiroContext.Mensalidades;

public sealed class TabelaMensalidade
{
    public Guid Id { get; }
    public int TurmaId { get; }
    public decimal Valor { get; }
}
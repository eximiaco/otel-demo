using CSharpFunctionalExtensions;
using Entity = OtelDemo.Common.Entity;

namespace OtelDemo.Inscricoes.Mensalidades;

public sealed class Mensalidade : Entity
{
    internal Mensalidade(Guid id, Guid inscricaoId, string responsavelFinanceiro, decimal valor)
    {
        Id = id;
        InscricaoId = inscricaoId;
        ResponsavelFinanceiro = responsavelFinanceiro;
        Valor = valor;
    }

    public Guid Id { get; }
    public Guid InscricaoId { get; }
    public string ResponsavelFinanceiro { get; }
    public decimal Valor { get; }

    public static Result<Mensalidade> Criar(Guid inscricaoId, string resposnavel, decimal valor)
    {
        return new Mensalidade(Guid.NewGuid(), inscricaoId, resposnavel, valor);
    }
}
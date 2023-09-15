using System.Linq.Expressions;
using OtelDemo.Common;

namespace OtelDemo.Inscricoes.FinanceiroContext.Precificacao;

public sealed class TabelaMensalidade : Entity
{
    public Guid Id { get; }
    public int TurmaId { get; }
    public decimal ValorBase { get; }
    public IRegraCalculoVencimento RegraVencimento { get; }
    public IEnumerable<IRegraDesconto> Descontos { get; }

    public decimal CalcularValorMensalidade(Guid inscricao)
    {
        var contexto = new ContextoCalculo();
        var descontoTotal = Descontos.Sum(desconto => 
            desconto.PossoAplicar(inscricao, contexto) 
                ? desconto.Calcular(ValorBase, inscricao, contexto)
                : 0m);
        return ValorBase - descontoTotal;
    }
}

public sealed class ContextoCalculo
{
    public ContextoCalculo()
    {
        Propriedades = new Dictionary<string, object>();
    }
    public Dictionary<string, object> Propriedades { get; }

    public void AdicionarContexto(string chave, object valor)
    {
        Propriedades.Add(chave, valor);
    }
}


public interface IRegraCalculoVencimento
{
    DateTime CalcularVencimento(DateTime contratacaoEm, int diaVencimento);
}

public interface IRegraDesconto
{
    bool PossoAplicar(Guid inscricao, ContextoCalculo contextoCalculo);
    decimal Calcular(decimal valorBase, Guid inscricao, ContextoCalculo contextoCalculo);
}
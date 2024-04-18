using CSharpFunctionalExtensions;

namespace OtelDemo.Domain.InscricoesContext.Inscricoes.Comandos;

public class CancelarInscricaoComando
{
    private CancelarInscricaoComando(Guid inscricaoId)
    {
        InscricaoId = inscricaoId;
    }
    
    public Guid InscricaoId { get; }

    public static Result<CancelarInscricaoComando> Criar(Guid inscricaoId)
    {
        return new CancelarInscricaoComando(inscricaoId);
    }
}
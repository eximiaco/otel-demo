using OtelDemo.Common;

namespace OtelDemo.Inscricoes.InscricoesContext.Inscricoes.Eventos;

public class InscricaoCanceladaEvento : INotification
{
    public InscricaoCanceladaEvento(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}

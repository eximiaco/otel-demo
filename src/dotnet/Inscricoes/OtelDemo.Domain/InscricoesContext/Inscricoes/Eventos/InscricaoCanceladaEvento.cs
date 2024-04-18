using OtelDemo.Common;

namespace OtelDemo.Domain.InscricoesContext.Inscricoes.Eventos;

public class InscricaoCanceladaEvento : INotification
{
    public InscricaoCanceladaEvento(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}

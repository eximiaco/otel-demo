using OtelDemo.Common;

namespace OtelDemo.Domain.AcessoContext.Permissoes;

public sealed class PermissaoAcesso : Entity
{
    public PermissaoAcesso(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
    
}
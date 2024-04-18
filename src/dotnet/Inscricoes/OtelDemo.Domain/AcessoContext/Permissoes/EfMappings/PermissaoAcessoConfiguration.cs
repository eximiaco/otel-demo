using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OtelDemo.Domain.AcessoContext.Permissoes.EfMappings;

public sealed class PermissaoAcessoConfiguration : IEntityTypeConfiguration<PermissaoAcesso>
{
    public void Configure(EntityTypeBuilder<PermissaoAcesso> builder)
    {
        builder.ToTable("Permissoes", "Acesso");
        builder.HasKey(p => p.Id);
    }
}
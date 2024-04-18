using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OtelDemo.Inscricoes.FinanceiroContext.Mensalidades.EfMappings;

public sealed class MensalidadeConfiguration : IEntityTypeConfiguration<Mensalidade>
{
    public void Configure(EntityTypeBuilder<Mensalidade> builder)
    {
        builder.ToTable("Mensalidades", "Financeiro");
        builder.HasKey(p => p.Id);
        builder.Property(c => c.InscricaoId).IsRequired(true);
        builder.Property(c => c.ResponsavelFinanceiro);
        builder.Property(c => c.Valor).IsRequired(true);
    }
}
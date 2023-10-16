using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OtelDemo.Domain.InscricoesContext.Inscricoes.EfMappings;

public class TurmasConfigurations: IEntityTypeConfiguration<Turma>
{
    public void Configure(EntityTypeBuilder<Turma> builder)
    {
        builder.ToTable("Turmas", "Inscricoes");
        builder.HasKey(p => p.Id);
        builder.Property(c => c.Vagas);
        //builder.Property<DateTime>("DataCadastro");
        //builder.Property<DateTime>("DataUltimaAlteracao");
    }
}
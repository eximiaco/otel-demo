using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OtelDemo.Domain.InscricoesContext.Inscricoes.EfMappings;

public class InscricoesConfigurations: IEntityTypeConfiguration<Inscricao>
{
    public void Configure(EntityTypeBuilder<Inscricao> builder)
    {
        builder.ToTable("Matriculas", "Inscricoes");
        builder.HasKey(p => p.Id);
        builder.Property(c => c.Aluno).IsRequired(true);
        builder.Property(c => c.Responsavel).IsRequired(true);
        builder.Property(c => c.Turma).IsRequired();
        builder.Ignore(c => c.Ativa);
        //builder.Property<DateTime>("DataCadastro");
        //builder.Property<DateTime>("DataUltimaAlteracao");
    }
}
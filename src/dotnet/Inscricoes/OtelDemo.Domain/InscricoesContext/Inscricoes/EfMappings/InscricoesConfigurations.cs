using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OtelDemo.Inscricoes.InscricoesContext.Inscricoes.EfMappings;

public class InscricoesConfigurations: IEntityTypeConfiguration<Inscricao>
{
    public void Configure(EntityTypeBuilder<Inscricao> builder)
    {
        builder.ToTable("inscricoes");
        builder.HasKey(p => p.Id);
        builder.Property(c => c.Aluno).IsRequired(true);
        builder.Property(c => c.Responsavel).IsRequired(true);
        builder
            .HasOne(c => c.Turma)
            .WithMany()
            .HasForeignKey("turma")
            .HasPrincipalKey(c=> c.Id)
            .IsRequired(true);
        //builder.Property<DateTime>("DataCadastro");
        //builder.Property<DateTime>("DataUltimaAlteracao");
    }
}
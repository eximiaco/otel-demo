using Microsoft.EntityFrameworkCore;
using OtelDemo.Common.ServiceBus;
using OtelDemo.Common.UoW;
using OtelDemo.Inscricoes.InscricoesContext.Inscricoes;
using OtelDemo.Inscricoes.InscricoesContext.Inscricoes.EfMappings;

namespace OtelDemo.Inscricoes.InscricoesContext.Infrastructure;

public class InscricoesDbContext: DbContext, IUnitOfWork
{
    private readonly IServiceBus _serviceBus;
    public const string DEFAULT_SCHEMA = "inscricoes";
    
    public InscricoesDbContext(DbContextOptions<InscricoesDbContext> options) : base(options) { }
    
    public InscricoesDbContext(DbContextOptions<InscricoesDbContext> options, IServiceBus serviceBus) : base(options)
    {
        _serviceBus = serviceBus ?? throw new ArgumentNullException(nameof(serviceBus));
        
        System.Diagnostics.Debug.WriteLine("InscricoesDbContext::ctor ->" + this.GetHashCode());
    }
    
    public DbSet<Inscricao> Inscricoes { get; set; }
    public DbSet<Turma> Turmas { get; set; }
    
    public async Task Salvar(CancellationToken cancellationToken)
    {
        try
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if ((item.State == EntityState.Modified || item.State == EntityState.Added)
                    && item.Properties.Any(c => c.Metadata.Name == "DataUltimaAlteracao"))
                    item.Property("DataUltimaAlteracao").CurrentValue = DateTime.UtcNow;

                if (item.State == EntityState.Added)
                    if (item.Properties.Any(c => c.Metadata.Name == "DataCadastro") && item.Property("DataCadastro").CurrentValue.GetType() != typeof(DateTime))
                        item.Property("DataCadastro").CurrentValue = DateTime.UtcNow;
            }
            var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await _serviceBus.DispatchDomainEventsAsync(this).ConfigureAwait(false);
        }
        catch (DbUpdateException e)
        {
            throw new Exception();
        }
        catch (Exception)
        {
            throw;
        }

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InscricoesConfigurations());
        modelBuilder.ApplyConfiguration(new TurmasConfigurations());
        modelBuilder.LowercaseRelationalTableAndPropertyNames();
    }
}

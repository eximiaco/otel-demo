using Microsoft.EntityFrameworkCore;
using OtelDemo.Common.ServiceBus;
using OtelDemo.Domain.InscricoesContext.Inscricoes;
using OtelDemo.Domain.InscricoesContext.Inscricoes.EfMappings;

namespace OtelDemo.Domain.InscricoesContext.Infrastructure;

public class InscricoesDbContext: DbContext
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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
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
            return result;
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
    }
}

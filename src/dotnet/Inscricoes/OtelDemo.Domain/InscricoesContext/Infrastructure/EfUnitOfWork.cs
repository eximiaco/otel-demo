using OtelDemo.Common.UoW;

namespace OtelDemo.Domain.InscricoesContext.Infrastructure;

public class EfUnitOfWork: IUnitOfWork
{
    private readonly IEfDbContextAccessor<InscricoesDbContext> _efDbContextAccessor;

    public EfUnitOfWork(IEfDbContextAccessor<InscricoesDbContext> efDbContextAccessor)
    {
        _efDbContextAccessor = efDbContextAccessor;
    }
    
    public async Task Commit(CancellationToken cancellationToken)
    {
        await _efDbContextAccessor.Get().SaveChangesAsync(cancellationToken);
    }
}
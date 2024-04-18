using OtelDemo.Common.UoW;

namespace OtelDemo.Inscricoes.FinanceiroContext.Infrastructure;

public class EfUnitOfWork: IUnitOfWork
{
    private readonly IEfDbContextAccessor<FinanceiroDbContext> _efDbContextAccessor;

    public EfUnitOfWork(IEfDbContextAccessor<FinanceiroDbContext> efDbContextAccessor)
    {
        _efDbContextAccessor = efDbContextAccessor;
    }
    
    public async Task Commit(CancellationToken cancellationToken)
    {
        await _efDbContextAccessor.Get().SaveChangesAsync(cancellationToken);
    }
}
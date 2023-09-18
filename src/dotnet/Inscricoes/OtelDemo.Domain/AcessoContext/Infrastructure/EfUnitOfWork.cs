using OtelDemo.Common.UoW;

namespace OtelDemo.Domain.AcessoContext.Infrastructure;

public class EfUnitOfWork: IUnitOfWork
{
    private readonly IEfDbContextAccessor<AcessoDbContext> _efDbContextAccessor;

    public EfUnitOfWork(IEfDbContextAccessor<AcessoDbContext> efDbContextAccessor)
    {
        _efDbContextAccessor = efDbContextAccessor;
    }
    
    public async Task Commit(CancellationToken cancellationToken)
    {
        await _efDbContextAccessor.Get().SaveChangesAsync(cancellationToken);
    }
}
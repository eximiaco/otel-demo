namespace OtelDemo.Common.UoW;

public class UnitOfWorkMock : IUnitOfWork
{
    public Task Commit(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
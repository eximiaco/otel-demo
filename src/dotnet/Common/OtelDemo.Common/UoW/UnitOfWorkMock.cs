namespace OtelDemo.Common.UoW;

public class UnitOfWorkMock : IUnitOfWork
{
    public Task Salvar(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
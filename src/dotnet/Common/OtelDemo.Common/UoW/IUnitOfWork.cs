namespace OtelDemo.Common.UoW;

public interface IUnitOfWork
{
    Task Commit(CancellationToken cancellationToken);
}
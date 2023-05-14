namespace OtelDemo.Common.UoW;

public interface IUnitOfWork
{
    Task Salvar(CancellationToken cancellationToken);
}
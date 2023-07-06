using Microsoft.EntityFrameworkCore;

namespace OtelDemo.Common.UoW;

public interface IEfDbContextAccessor<T> : IDisposable where T : DbContext
{
    void Register(T context);
    T Get();
    void Clear();
}
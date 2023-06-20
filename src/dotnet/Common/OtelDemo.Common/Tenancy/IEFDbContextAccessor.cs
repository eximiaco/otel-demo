using Microsoft.EntityFrameworkCore;

namespace OtelDemo.Common.Tenancy;

public interface IEFDbContextAccessor<T> : IDisposable where T : DbContext
{
    void Register(T context);
    T Get();
    void Clear();
}
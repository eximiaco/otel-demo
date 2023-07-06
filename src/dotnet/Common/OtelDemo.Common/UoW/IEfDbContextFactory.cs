using Microsoft.EntityFrameworkCore;

namespace OtelDemo.Common.UoW;

public interface IEfDbContextFactory<T> where T : DbContext
{
    Task<T> CriarAsync(string codigoTenant);
}
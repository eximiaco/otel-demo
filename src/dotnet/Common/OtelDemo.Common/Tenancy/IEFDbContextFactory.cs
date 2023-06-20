using Microsoft.EntityFrameworkCore;

namespace OtelDemo.Common.Tenancy;

public interface IEFDbContextFactory<T> where T : DbContext
{
    Task<T> CriarAsync(string codigoTenant);
}
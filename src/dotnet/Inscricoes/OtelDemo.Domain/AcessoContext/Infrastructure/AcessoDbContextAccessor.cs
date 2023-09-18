using OtelDemo.Common.UoW;

namespace OtelDemo.Domain.AcessoContext.Infrastructure;

public sealed class AcessoDbContextAccessor: IEfDbContextAccessor<AcessoDbContext>
{
    private AcessoDbContext _contexto = null!;
    private bool _disposed = false;

    public AcessoDbContext Get()
    {
        return _contexto ?? throw new InvalidOperationException("Contexto deve ser registrado!");
    }

    public void Register(AcessoDbContext context)
    {
        _disposed = false;
        _contexto = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Clear()
    {
        Dispose(true);
    }

    public void Dispose()
    {
        Dispose(true);
        // ReSharper disable once GCSuppressFinalizeForTypeWithoutDestructor
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
            _contexto?.Dispose();
        _contexto = null!;
        _disposed = true;
    }
}
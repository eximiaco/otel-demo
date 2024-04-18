using OtelDemo.Common.UoW;

namespace OtelDemo.Inscricoes.FinanceiroContext.Infrastructure;

public sealed class FinanceiroDbContextAccessor: IEfDbContextAccessor<FinanceiroDbContext>
{
    private FinanceiroDbContext _contexto = null!;
    private bool _disposed = false;

    public FinanceiroDbContext Get()
    {
        return _contexto ?? throw new InvalidOperationException("Contexto deve ser registrado!");
    }

    public void Register(FinanceiroDbContext context)
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
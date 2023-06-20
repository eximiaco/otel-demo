using OtelDemo.Common.Tenancy;

namespace OtelDemo.Inscricoes.Domain.Infrastructure;

public sealed class InscricoesDbContextAccessor: IEFDbContextAccessor<InscricoesDbContext>
{
    private InscricoesDbContext _contexto = null!;
    private bool _disposed = false;

    public InscricoesDbContext Get()
    {
        return _contexto ?? throw new InvalidOperationException("Contexto deve ser registrado!");
    }

    public void Register(InscricoesDbContext context)
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
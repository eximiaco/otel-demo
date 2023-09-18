using OtelDemo.Common;
using OtelDemo.Common.UoW;
using OtelDemo.Inscricoes.FinanceiroContext.Infrastructure;

namespace OtelDemo.Inscricoes.FinanceiroContext.Mensalidades;

public sealed class MensalidadesRepositorio : IService<MensalidadesRepositorio>
{
    private readonly IEfDbContextAccessor<FinanceiroDbContext> _dbContext;

    public MensalidadesRepositorio(IEfDbContextAccessor<FinanceiroDbContext> dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Adicionar(IEnumerable<Mensalidade> mensalidades, CancellationToken cancellationToken)
    {
        await _dbContext.Get().Mensalidades.AddRangeAsync(mensalidades, cancellationToken);
    }
}
using Dapper;
using Microsoft.EntityFrameworkCore;
using OtelDemo.Common;
using OtelDemo.Common.UoW;
using OtelDemo.Domain.InscricoesContext.Infrastructure;

namespace OtelDemo.Domain.InscricoesContext.QueryModel;

public sealed class InscricoesQuery : IService<InscricoesQuery>
{
    private readonly IEfDbContextAccessor<InscricoesDbContext> _dbContext;

    public InscricoesQuery(IEfDbContextAccessor<InscricoesDbContext> dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<InscricaoSummayViewModel> Get(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Get().Database.GetDbConnection()
            .QueryFirstOrDefaultAsync<InscricaoSummayViewModel>("SELECT Id, Ativa FROM Inscricoes.Matriculas WHERE Id = @id",
                new {id});
    }
}
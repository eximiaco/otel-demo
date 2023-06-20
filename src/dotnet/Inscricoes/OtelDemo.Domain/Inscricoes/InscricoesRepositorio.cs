using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using OtelDemo.Common;
using OtelDemo.Common.OpenTelemetry;
using OtelDemo.Common.Tenancy;
using OtelDemo.Common.UoW;
using OtelDemo.Inscricoes.Domain.Infrastructure;

namespace OtelDemo.Inscricoes.Domain.Inscricoes;

public sealed class InscricoesRepositorio : IService<InscricoesRepositorio>
{
    private readonly ITelemetryFactory _telemetryFactory;
    private readonly IEFDbContextAccessor<InscricoesDbContext> _dbContext;

    public InscricoesRepositorio(
        ITelemetryFactory telemetryFactory,
        IEFDbContextAccessor<InscricoesDbContext> dbContext)
    {
        _telemetryFactory = telemetryFactory;
        _dbContext = dbContext;
    }

    public IUnitOfWork UnitOfWork => _dbContext.Get();
    
    public async Task<bool> AlunoExiste(string aluno)
    {
        return true;
    }
    
    public async Task<bool> ResponsavelExiste(string responsavel)
    {
        return true;
    }

    public async Task<Maybe<Turma>> RecuperarTurma(int id)
    {
        var turma = await _dbContext.Get().Turmas.FirstOrDefaultAsync(c => c.Id == id);
        return turma ?? Maybe<Turma>.None;
    }

    public async Task Adicionar(Inscricao inscricao)
    {
        using var activity = _telemetryFactory.Create($"{nameof(InscricoesRepositorio)}.{nameof(Adicionar)}");
        activity.AddTag("inscricao", inscricao.Id.ToString());
        await _dbContext.Get().Inscricoes.AddAsync(inscricao);
        activity.AddInformationEvent("Inscricao adicionada no repositorio {inscricao}", new { inscricao = inscricao.Id.ToString() });
    }
}
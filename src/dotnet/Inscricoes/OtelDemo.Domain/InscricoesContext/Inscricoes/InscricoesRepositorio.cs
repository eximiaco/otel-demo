using System.Data.SqlClient;
using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.EntityFrameworkCore;
using OtelDemo.Common;
using OtelDemo.Common.OpenTelemetry;
using OtelDemo.Common.UoW;
using OtelDemo.Inscricoes.InscricoesContext.Infrastructure;

namespace OtelDemo.Inscricoes.InscricoesContext.Inscricoes;

public sealed class InscricoesRepositorio : IService<InscricoesRepositorio>
{
    private readonly ITelemetryFactory _telemetryFactory;
    private readonly IEfDbContextAccessor<InscricoesDbContext> _dbContext;

    public InscricoesRepositorio(
        ITelemetryFactory telemetryFactory,
        IEfDbContextAccessor<InscricoesDbContext> dbContext)
    {
        _telemetryFactory = telemetryFactory;
        _dbContext = dbContext;
    }

    public async Task<bool> AlunoExiste(string aluno)
    {
        var result = await _dbContext.Get().Database.GetDbConnection()
            .QueryFirstOrDefaultAsync<string>("SELECT codigo FROM public.alunos WHERE codigo = @aluno",
            new {aluno});
        return result == aluno;
    }
    
    public async Task<bool> ResponsavelExiste(string responsavel)
    {
        var result = await _dbContext.Get().Database.GetDbConnection()
            .QueryFirstOrDefaultAsync<string>("SELECT codigo FROM public.responsaveis WHERE codigo = @responsavel",
                new {responsavel});
        return result == responsavel;
    }

    public async Task<Maybe<Turma>> RecuperarTurma(int id, CancellationToken cancellationToken)
    {
        var turma = await _dbContext.Get().Turmas.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        return turma ?? Maybe<Turma>.None;
    }

    public async Task Adicionar(Inscricao inscricao, CancellationToken cancellationToken)
    {
        using var activity = _telemetryFactory.Create($"{nameof(InscricoesRepositorio)}.{nameof(Adicionar)}");
        activity.AddTag("inscricao", inscricao.Id.ToString());
        await _dbContext.Get().Inscricoes.AddAsync(inscricao, cancellationToken);
        activity.AddInformationEvent("Inscricao adicionada no repositorio {inscricao}", new { inscricao = inscricao.Id.ToString() });
    }

    public async Task<Inscricao> Recuperar(Guid comandoInscricaoId)
    {
        throw new NotImplementedException();
    }
}
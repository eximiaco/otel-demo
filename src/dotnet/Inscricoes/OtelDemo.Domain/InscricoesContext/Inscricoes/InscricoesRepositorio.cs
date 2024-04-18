using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.EntityFrameworkCore;
using OtelDemo.Common;
using OtelDemo.Common.OpenTelemetry;
using OtelDemo.Common.UoW;
using OtelDemo.Domain.InscricoesContext.Infrastructure;
using Serilog;

namespace OtelDemo.Domain.InscricoesContext.Inscricoes;

public sealed class InscricoesRepositorio : IService<InscricoesRepositorio>
{
    private readonly ITelemetryFactory _telemetryFactory;
    private readonly IEfDbContextAccessor<InscricoesDbContext> _dbContext;
    private readonly ILogger _logger;

    public InscricoesRepositorio(
        ITelemetryFactory telemetryFactory,
        IEfDbContextAccessor<InscricoesDbContext> dbContext,
        ILogger logger)
    {
        _telemetryFactory = telemetryFactory;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<bool> AlunoExiste(string aluno)
    {
        var result = await _dbContext.Get().Database.GetDbConnection()
            .QueryFirstOrDefaultAsync<string>("SELECT codigo FROM Inscricoes.Alunos WHERE codigo = @aluno",
            new {aluno});
        
        if(result != aluno)
            _logger.Warning("Aluno não foi localizado no banco de dados");
        
        return result == aluno;
    }
    
    public async Task<bool> ResponsavelExiste(string responsavel)
    {
        var result = await _dbContext.Get().Database.GetDbConnection()
            .QueryFirstOrDefaultAsync<string>("SELECT codigo FROM Inscricoes.Responsaveis WHERE codigo = @responsavel",
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
        activity.AddLogInformationAndEvent("Inscricao adicionada no repositorio {inscricao}", new { inscricao = inscricao.Id.ToString() });
    }

    public async Task<Inscricao> Recuperar(Guid comandoInscricaoId)
    {
        throw new NotImplementedException();
    }
}
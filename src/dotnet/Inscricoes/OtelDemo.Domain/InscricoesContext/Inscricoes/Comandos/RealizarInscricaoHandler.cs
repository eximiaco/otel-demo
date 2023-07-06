using CSharpFunctionalExtensions;
using OtelDemo.Common;
using OtelDemo.Common.OpenTelemetry;
using OtelDemo.Common.UoW;

namespace OtelDemo.Inscricoes.InscricoesContext.Inscricoes.Comandos;

public class RealizarInscricaoHandler : IService<RealizarInscricaoHandler>
{
    private readonly ITelemetryFactory _telemetryFactory;
    private readonly InscricoesRepositorio _inscricoesRepositorio;
    private readonly IUnitOfWork _unitOfWork;

    public RealizarInscricaoHandler(
        ITelemetryFactory telemetryFactory,
        InscricoesRepositorio inscricoesRepositorio,
        IUnitOfWork unitOfWork)
    {
        _telemetryFactory = telemetryFactory;
        _inscricoesRepositorio = inscricoesRepositorio;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Executar(RealizarInscricaoComando comando, CancellationToken cancellationToken)
    {
        using var activity = _telemetryFactory.Create($"{nameof(RealizarInscricaoHandler)}.{nameof(Executar)}");
        activity
            .AddTag("aluno", comando.Aluno)
            .AddTag("responsavel", comando.Responsavel)
            .AddTag("turma", comando.Turma);

        if (!await _inscricoesRepositorio.AlunoExiste(comando.Aluno))
        {
            activity.SetError("Falha ao realizar inscricao [{error}]", new { error = "Aluno inválido"});
            return Result.Failure("Aluno inválido");
        }

        if (!await _inscricoesRepositorio.ResponsavelExiste(comando.Responsavel))
        {
            activity.SetError("Falha ao realizar inscricao [{error}]", new { error = "Responsável inválido"});
            return Result.Failure("Responsável inválido");
        }
        
        var turma = await _inscricoesRepositorio.RecuperarTurma(comando.Turma, cancellationToken);
        if (turma.HasNoValue)
        {
            activity.SetError("Falha ao realizar inscricao [{error}]", new { error = "Turma inválida"});
            return Result.Failure("Turma inválida");
        }
            
        var inscricao = Inscricao.CriarNova(comando.Aluno, comando.Responsavel, turma.Value);
        if (inscricao.IsFailure)
        {
            activity.SetError("Falha ao realizar inscricao [{error}]", new { error = inscricao.Error});
            return Result.Failure(inscricao.Error);
        }
            
        await _inscricoesRepositorio.Adicionar(inscricao.Value, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        activity
            .AddTag("inscricao", inscricao.Value.Id)
            .SetSucess("Inscricao {inscricao} realizada com sucesso", new { inscricao = inscricao.Value.Id.ToString()});

        return Result.Success();
    }
}
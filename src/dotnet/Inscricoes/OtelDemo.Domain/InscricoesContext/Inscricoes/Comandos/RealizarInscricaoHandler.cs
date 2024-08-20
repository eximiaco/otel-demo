using CSharpFunctionalExtensions;
using OtelDemo.Common;
using OtelDemo.Common.UoW;
using OtelDemo.Domain.InscricoesContext.Inscricoes.Telemetria;
using Serilog;

namespace OtelDemo.Domain.InscricoesContext.Inscricoes.Comandos;

public class RealizarInscricaoHandler : IService<RealizarInscricaoHandler>
{
    private readonly IRealizarInscricaoTelemetry _realizarInscricaoTelemetry;
    private readonly InscricoesRepositorio _inscricoesRepositorio;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public RealizarInscricaoHandler(
        IRealizarInscricaoTelemetry realizarInscricaoTelemetry,
        InscricoesRepositorio inscricoesRepositorio,
        IUnitOfWork unitOfWork,
        Serilog.ILogger logger)
    {
        _realizarInscricaoTelemetry = realizarInscricaoTelemetry;
        _inscricoesRepositorio = inscricoesRepositorio;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Result> Executar(RealizarInscricaoComando comando, CancellationToken cancellationToken)
    {
        _realizarInscricaoTelemetry.NovaInscricaoRecebida(comando);
        
        _logger.Information("Recebeu o comando {responsavel}", comando.Responsavel);


        if (!await _inscricoesRepositorio.AlunoExiste(comando.Aluno))
            return _realizarInscricaoTelemetry.AlunoNaoLocalizado(comando);
        
        _realizarInscricaoTelemetry.AlunoLocalizado();
        
        if (!await _inscricoesRepositorio.ResponsavelExiste(comando.Responsavel))
            _realizarInscricaoTelemetry.ResponsavelNaoLocalizado(comando);

        _realizarInscricaoTelemetry.ResponsavelLocalizado();
        
        var turma = await _inscricoesRepositorio.RecuperarTurma(comando.Turma, cancellationToken);
        if (turma.HasNoValue)
            return _realizarInscricaoTelemetry.TurmaNaoLocalizada(comando);

        _realizarInscricaoTelemetry.TurmaLocalizada(turma.Value);
        
        var inscricao = Inscricao.CriarNova(comando.Aluno, comando.Responsavel, turma.Value.Id);
        if (inscricao.IsFailure)
            return _realizarInscricaoTelemetry.NaoFoiPossivelCriarInscricao(comando, inscricao.Error);
            
        await _inscricoesRepositorio.Adicionar(inscricao.Value, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        _realizarInscricaoTelemetry.InscricaoRelizada(inscricao.Value);
        
        return Result.Success();
    }
}
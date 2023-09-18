using CSharpFunctionalExtensions;
using OtelDemo.Common;
using OtelDemo.Common.OpenTelemetry;
using OtelDemo.Common.UoW;
using OtelDemo.Inscricoes.InscricoesContext.Inscricoes.Telemetria;

namespace OtelDemo.Inscricoes.InscricoesContext.Inscricoes.Comandos;

public class RealizarInscricaoHandler : IService<RealizarInscricaoHandler>
{
    private readonly IRealizarInscricaoTelemetry _realizarInscricaoTelemetry;
    private readonly InscricoesRepositorio _inscricoesRepositorio;
    private readonly IUnitOfWork _unitOfWork;

    public RealizarInscricaoHandler(
        IRealizarInscricaoTelemetry realizarInscricaoTelemetry,
        InscricoesRepositorio inscricoesRepositorio,
        IUnitOfWork unitOfWork)
    {
        _realizarInscricaoTelemetry = realizarInscricaoTelemetry;
        _inscricoesRepositorio = inscricoesRepositorio;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Executar(RealizarInscricaoComando comando, CancellationToken cancellationToken)
    {
        _realizarInscricaoTelemetry.NovaInscricaoRecebida(comando);
        
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
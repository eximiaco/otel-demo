using CSharpFunctionalExtensions;
using OtelDemo.Common;
using OtelDemo.Common.OpenTelemetry;
using OtelDemo.Common.UoW;

namespace OtelDemo.Inscricoes.HttpService.Domain.Inscricoes;

public sealed class InscricoesRepositorio : IService<InscricoesRepositorio>
{
    private readonly ITelemetryFactory _telemetryFactory;

    public InscricoesRepositorio(
        ITelemetryFactory telemetryFactory,
        IUnitOfWork unitOfWork)
    {
        _telemetryFactory = telemetryFactory;
        UnitOfWork = unitOfWork;
    }
    
    public IUnitOfWork UnitOfWork { get; }
    
    public async Task<bool> AlunoExiste(string aluno)
    {
        return aluno != "12312312312";
    }
    
    public async Task<bool> ResponsavelExiste(string responsavel)
    {
        return responsavel != "12312312312";
    }

    public async Task<Maybe<Turma>> RecuperarTurma(int turma)
    {
        return turma == 123 
            ? Maybe<Turma>.None 
            : new Turma(turma, 21);
    }

    public void Adicionar(Inscricao inscricao)
    {
        using var activity = _telemetryFactory.Create($"{nameof(InscricoesRepositorio)}.{nameof(Adicionar)}");

        activity.AddTag("inscricao", inscricao.Id.ToString());
        
        activity.AddInformationEvent("Inscricao realizada {inscricao}", new { inscricao = inscricao.Id.ToString() });
    }
}
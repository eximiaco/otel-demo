using CSharpFunctionalExtensions;
using OtelDemo.Domain.InscricoesContext.Inscricoes.Comandos;

namespace OtelDemo.Domain.InscricoesContext.Inscricoes.Telemetria;

public interface IRealizarInscricaoTelemetry : IDisposable
{
    void NovaInscricaoRecebida(RealizarInscricaoComando comando);
    Result AlunoNaoLocalizado(RealizarInscricaoComando comando);
    void AlunoLocalizado();
    Result ResponsavelNaoLocalizado(RealizarInscricaoComando comando);
    void ResponsavelLocalizado();
    Result TurmaNaoLocalizada(RealizarInscricaoComando comando);
    void TurmaLocalizada(Turma turma);
    Result NaoFoiPossivelCriarInscricao(RealizarInscricaoComando comando, string error);
    void InscricaoRelizada(Inscricao inscricao);
}
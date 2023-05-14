using CSharpFunctionalExtensions;

namespace OtelDemo.Inscricoes.HttpService.Domain.Inscricoes.Comandos;

public record RealizarInscricaoComando
{
    private RealizarInscricaoComando(string aluno, string responsavel, int turma)
    {
        Aluno = aluno;
        Responsavel = responsavel;
        Turma = turma;
    }
    
    public string Aluno { get; }
    public string Responsavel { get; }
    public int Turma { get; }

    public static Result<RealizarInscricaoComando> Criar(string aluno, string responsavel, int turma)
    {
        var validacao = Result.Combine(
            Result.FailureIf(string.IsNullOrEmpty(aluno), "Aluno obrigatório"),
            Result.FailureIf(string.IsNullOrEmpty(responsavel), "Responsável obrigatório"),
            Result.FailureIf(turma <= 0, "Turma obrigatória"));
        return validacao.IsFailure 
            ? Result.Failure<RealizarInscricaoComando>(validacao.Error) 
            : new RealizarInscricaoComando(aluno, responsavel, turma);
    } 
}
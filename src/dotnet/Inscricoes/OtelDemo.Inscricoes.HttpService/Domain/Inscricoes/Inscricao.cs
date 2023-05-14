using CSharpFunctionalExtensions;

namespace OtelDemo.Inscricoes.HttpService.Domain.Inscricoes;

public sealed class Inscricao
{
    private Inscricao(Guid id, string aluno, string responsavel, Turma turma)
    {
        Id = id;
        Aluno = aluno;
        Responsavel = responsavel;
        Turma = turma;
    }

    public Guid Id { get; }
    public string Aluno { get; }
    public string Responsavel { get; }
    public Turma Turma { get; }
    
    public static Result<Inscricao> CriarNova(string aluno, string responsavel, Turma turma)
    {
        return new Inscricao(Guid.NewGuid(), aluno, responsavel, turma);
    }
}
using CSharpFunctionalExtensions;
using Entity = OtelDemo.Common.Entity;

namespace OtelDemo.Inscricoes.Domain.Inscricoes;

public sealed class Inscricao : Entity
{
    private Inscricao(){}
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
using CSharpFunctionalExtensions;
using OtelDemo.Inscricoes.Domain.Inscricoes.Eventos;
using Entity = OtelDemo.Common.Entity;

namespace OtelDemo.Inscricoes.Domain.Inscricoes;

public sealed class Inscricao : Entity
{
    private Inscricao(){}
    private Inscricao(Guid id, string aluno, string responsavel, Turma turma, bool Ativa)
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
    public bool Ativa { get; private set; }
    
    public static Result<Inscricao> CriarNova(string aluno, string responsavel, Turma turma)
    {
        var inscricao =new Inscricao(Guid.NewGuid(), aluno, responsavel, turma, true);
        inscricao.AddDomainEvent(new InscricaoRealizadaEvento(inscricao.Id, inscricao.Responsavel) );
        return inscricao;
    }

    public Result Cancelar()
    {
        if (!Ativa)
            return Result.Failure("Inscricao ja cancelada");
        Ativa = false;
        this.AddDomainEvent(new InscricaoCanceladaEvento(Id));
        return Result.Success();
    }
}
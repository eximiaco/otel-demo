using CSharpFunctionalExtensions;

namespace OtelDemo.Inscricoes.Domain.Inscricoes.Comandos;

public class CancelarInscricaoHandler
{
    private readonly InscricoesRepositorio _repositorio;

    public CancelarInscricaoHandler(InscricoesRepositorio repositorio)
    {
        _repositorio = repositorio;
    }
    
    public async Task<Result> Executar(CancelarInscricaoComando comando, CancellationToken cancellationToken)
    {
        var inscricao = await _repositorio.Recuperar(comando.InscricaoId);
        inscricao.Cancelar();
        await _repositorio.UnitOfWork.Salvar(cancellationToken);
        return Result.Success();
    }
}
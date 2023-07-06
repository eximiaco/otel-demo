using CSharpFunctionalExtensions;
using OtelDemo.Common.UoW;

namespace OtelDemo.Inscricoes.InscricoesContext.Inscricoes.Comandos;

public class CancelarInscricaoHandler
{
    private readonly InscricoesRepositorio _repositorio;
    private readonly IUnitOfWork _unitOfWork;

    public CancelarInscricaoHandler(
        InscricoesRepositorio repositorio,
        IUnitOfWork unitOfWork)
    {
        _repositorio = repositorio;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Executar(CancelarInscricaoComando comando, CancellationToken cancellationToken)
    {
        var inscricao = await _repositorio.Recuperar(comando.InscricaoId);
        inscricao.Cancelar();
        await  _unitOfWork.Commit(cancellationToken);
        return Result.Success();
    }
}
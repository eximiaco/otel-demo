using CSharpFunctionalExtensions;
using OtelDemo.Common.UoW;
using OtelDemo.Domain.AcessoContext.Infrastructure;
using OtelDemo.Inscricoes.FinanceiroContext.Infrastructure;
using OtelDemo.Inscricoes.FinanceiroContext.Mensalidades;

namespace OtelDemo.Domain.AcessoContext.Permissoes.Aplicacao;

public sealed class DarPermissaoAcessoParaNovaInscricaoHandler
{
    private readonly IEfDbContextFactory<AcessoDbContext> _factory;
    private readonly IEfDbContextAccessor<AcessoDbContext> _accessor;
    private readonly IUnitOfWork _unitOfWork;

    public DarPermissaoAcessoParaNovaInscricaoHandler(
        IEfDbContextFactory<AcessoDbContext> factory,
        IEfDbContextAccessor<AcessoDbContext> accessor,
        IUnitOfWork unitOfWork)
    {
        _factory = factory;
        _accessor = accessor;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Executar(InscricaoRealizadaEvento evento, CancellationToken cancellationToken)
    {
        await using var contexto = await _factory.CriarAsync("");
        _accessor.Register(contexto);
        var acesso = new PermissaoAcesso(Guid.NewGuid());
        await contexto.Permissoes.AddAsync(acesso,cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        _accessor.Clear();
        
        return Result.Success();
    } 
}
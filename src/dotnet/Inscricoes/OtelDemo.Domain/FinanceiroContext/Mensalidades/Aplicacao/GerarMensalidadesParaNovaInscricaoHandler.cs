using CSharpFunctionalExtensions;
using OtelDemo.Common.UoW;
using OtelDemo.Inscricoes.FinanceiroContext.Infrastructure;

namespace OtelDemo.Inscricoes.FinanceiroContext.Mensalidades.Aplicacao;

public sealed class GerarMensalidadesParaNovaInscricaoHandler
{
    private readonly IEfDbContextFactory<FinanceiroDbContext> _factory;
    private readonly IEfDbContextAccessor<FinanceiroDbContext> _accessor;
    private readonly MensalidadesRepositorio _mensalidadesRepositorio;
    private readonly IUnitOfWork _unitOfWork;

    public GerarMensalidadesParaNovaInscricaoHandler(
        IEfDbContextFactory<FinanceiroDbContext> factory,
        IEfDbContextAccessor<FinanceiroDbContext> accessor,
        MensalidadesRepositorio mensalidadesRepositorio,
        IUnitOfWork unitOfWork)
    {
        _factory = factory;
        _accessor = accessor;
        _mensalidadesRepositorio = mensalidadesRepositorio;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Executar(InscricaoRealizadaEvento evento, CancellationToken cancellationToken)
    {
        await using var contexto = await _factory.CriarAsync("");
        _accessor.Register(contexto);
        var mensalidades = new List<Mensalidade>()
        {
            Mensalidade.Criar(evento.Id, evento.Responsavel, 100).Value,
            Mensalidade.Criar(evento.Id, evento.Responsavel, 100).Value,
            Mensalidade.Criar(evento.Id, evento.Responsavel, 100).Value
        };

        await _mensalidadesRepositorio.Adicionar(mensalidades, cancellationToken);

        await _unitOfWork.Commit(cancellationToken);
        _accessor.Clear();
        
        return Result.Success();
    } 
}
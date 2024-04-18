using OtelDemo.Common.UoW;
using OtelDemo.Inscricoes.FinanceiroContext.Infrastructure;
using Silverback.Messaging.Broker.Behaviors;

namespace OtelDemo.Financeiro.BrokerConsumer;

public class CreateEfContextConsumerBehavior: IConsumerBehavior
{
    private readonly IEfDbContextFactory<FinanceiroDbContext> _factory;
    private readonly IEfDbContextAccessor<FinanceiroDbContext> _accessor;

    public CreateEfContextConsumerBehavior(
        IEfDbContextFactory<FinanceiroDbContext> factory,
        IEfDbContextAccessor<FinanceiroDbContext> accessor)
    {
        _factory = factory;
        _accessor = accessor;
    }

    public int SortIndex => 150;

    public async Task HandleAsync(
        ConsumerPipelineContext context, 
        ConsumerBehaviorHandler next)
    {
        await using var contexto = await _factory.CriarAsync("");
        _accessor.Register(contexto);
        // Call the next delegate/middleware in the pipeline.
        await next(context);
        _accessor.Clear();
    }
}
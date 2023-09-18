using OtelDemo.Common.UoW;
using OtelDemo.Domain.AcessoContext.Infrastructure;
using Silverback.Messaging.Broker.Behaviors;

namespace OtelDemo.Acesso.BrokerConsumer;

public class CreateEfContextConsumerBehavior: IConsumerBehavior
{
    private readonly IEfDbContextFactory<AcessoDbContext> _factory;
    private readonly IEfDbContextAccessor<AcessoDbContext> _accessor;

    public CreateEfContextConsumerBehavior(
        IEfDbContextFactory<AcessoDbContext> factory,
        IEfDbContextAccessor<AcessoDbContext> accessor)
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
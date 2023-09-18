using OtelDemo.Common;
using OtelDemo.Common.ServiceBus;

namespace OtelDemo.Domain.AcessoContext.Infrastructure;

static class ServiceBusExtension
{
    public static async Task DispatchDomainEventsAsync(this IServiceBus serviceBus, AcessoDbContext ctx)
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await serviceBus.PublishAsync(domainEvent);
    }
}
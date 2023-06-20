using Silverback.Messaging.Publishing;

namespace OtelDemo.Common.ServiceBus.Silverback;

public class SilverbackServiceBus : IServiceBus
{
    private readonly IPublisher _publisher;

    public SilverbackServiceBus(IPublisher publisher)
    {
        _publisher = publisher;
    }
    
    public Task PublishAsync(object message)
    {
        return _publisher.PublishAsync(message);
    }
}
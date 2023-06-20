namespace OtelDemo.Common.ServiceBus;

public interface IServiceBus
{
    Task PublishAsync(object message);
}
using Confluent.Kafka;

namespace OtelDemo.Common.ServiceBus;

public class KafkaConfig
{
    public KafkaConfig()
    {
        Connection = new KafkaConnectionConfig(string.Empty, Guid.NewGuid().ToString(), 
            SecurityProtocol.Plaintext, string.Empty, string.Empty);
        Consumer = new KafkaConsumerConfig(Array.Empty<KafkaInboundConfig>().ToList());
    }
    public KafkaConnectionConfig Connection { get; set; }
    public KafkaConsumerConfig Consumer { get; set; }
}

public record KafkaConnectionConfig(string BootstrapServers, string GroupId, SecurityProtocol SecurityProtocol, string Username, string Password);

public record KafkaConsumerConfig(List<KafkaInboundConfig> Inbounds);

public record KafkaInboundConfig(string Topics, int AutoOffsetReset);
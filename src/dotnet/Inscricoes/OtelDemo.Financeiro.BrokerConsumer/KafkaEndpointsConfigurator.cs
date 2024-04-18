using Confluent.Kafka;
using OtelDemo.Common.ServiceBus;
using OtelDemo.Common.ServiceBus.Silverback;
using OtelDemo.Inscricoes.FinanceiroContext.Mensalidades.Aplicacao;
using Silverback.Messaging.Configuration;

namespace OtelDemo.Financeiro.BrokerConsumer;

public class KafkaEndpointsConfigurator: IEndpointsConfigurator
{
    private readonly KafkaConfig _kafkaConfig;

    public KafkaEndpointsConfigurator(KafkaConfig kafkaConfig)
    {
        _kafkaConfig = kafkaConfig;
    }
    
    public void Configure(IEndpointsConfigurationBuilder builder)
    {
        builder
            .AddKafkaEndpoints(endpoints => endpoints
                .Configure(config => config.Configure(_kafkaConfig))
                .AddInbound(endpoint =>
                    endpoints
                        .AddInbound(builderEndpoint =>
                            builderEndpoint
                                .ConsumeFrom("inscricoes")
                                .Configure(config =>
                                {
                                    config.GroupId = _kafkaConfig.Connection.GroupId;
                                    config.AutoOffsetReset = AutoOffsetReset.Latest;
                                })
                                .DisableMessageValidation()
                                .DeserializeJson(serializer => serializer.UseFixedType<InscricaoRealizadaEvento>()))));
    }
}
using Autofac;
using OtelDemo.Common;
using OtelDemo.Common.OpenTelemetry;
using OtelDemo.Common.ServiceBus;
using OtelDemo.Common.ServiceBus.Silverback;
using OtelDemo.Common.Tenancy;
using OtelDemo.Inscricoes.Domain.Infrastructure;

namespace OtelDemo.Inscricoes.BrokerConsumer;

public class ApplicationModule: Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(Program).Assembly)
            .AsClosedTypesOf(typeof(IService<>))
            .InstancePerLifetimeScope();

        builder
            .RegisterType<InscricoesDbContextFactory>()
            .As<IEFDbContextFactory<InscricoesDbContext>>()
            .InstancePerLifetimeScope();

        builder
            .RegisterType<InscricoesDbContextAccessor>()
            .As<IEFDbContextAccessor<InscricoesDbContext>>()
            .InstancePerLifetimeScope();
        
        builder
            .RegisterType<SilverbackServiceBus>()
            .As<IServiceBus>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
        builder.RegisterType<TelemetryFactory>().As<ITelemetryFactory>().InstancePerLifetimeScope();
    }
}
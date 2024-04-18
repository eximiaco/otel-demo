using Autofac;
using OtelDemo.Common;
using OtelDemo.Common.OpenTelemetry;
using OtelDemo.Common.ServiceBus;
using OtelDemo.Common.ServiceBus.Silverback;
using OtelDemo.Common.UoW;
using OtelDemo.Domain.AcessoContext.Infrastructure;
using OtelDemo.Inscricoes;
using OtelDemo.Inscricoes.InscricoesContext.Shared.Telemetria;
using EfUnitOfWork = OtelDemo.Domain.AcessoContext.Infrastructure.EfUnitOfWork;

namespace OtelDemo.Acesso.BrokerConsumer;

public class ApplicationModule: Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(Ambiente).Assembly)
            .AsClosedTypesOf(typeof(IService<>))
            .InstancePerLifetimeScope();

        builder
            .RegisterType<AcessoDbContextFactory>()
            .As<IEfDbContextFactory<AcessoDbContext>>()
            .InstancePerLifetimeScope();

        builder
            .RegisterType<AcessoDbContextAccessor>()
            .As<IEfDbContextAccessor<AcessoDbContext>>()
            .InstancePerLifetimeScope();
        
        builder
            .RegisterType<EfUnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();
        
        builder
            .RegisterType<SilverbackServiceBus>()
            .As<IServiceBus>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
        builder.RegisterType<TelemetryFactory>().As<ITelemetryFactory>().InstancePerLifetimeScope();
        builder.RegisterType<OtelMetrics>().As<OtelMetrics>().SingleInstance();
        builder.RegisterType<OtelVariables>().As<OtelVariables>().SingleInstance();
    }
}
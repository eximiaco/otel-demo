using Autofac;
using OtelDemo.Common;
using OtelDemo.Common.OpenTelemetry;
using OtelDemo.Common.ServiceBus;
using OtelDemo.Common.ServiceBus.Silverback;
using OtelDemo.Common.UoW;
using OtelDemo.Inscricoes;
using OtelDemo.Inscricoes.FinanceiroContext.Infrastructure;
using OtelDemo.Inscricoes.InscricoesContext.Shared.Telemetria;

namespace OtelDemo.Financeiro.HttpService.Infrastructure;

public class ApplicationModule: Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(Ambiente).Assembly)
            .AsClosedTypesOf(typeof(IService<>))
            .InstancePerLifetimeScope();

        builder
            .RegisterType<FinanceiroDbContextFactory>()
            .As<IEfDbContextFactory<FinanceiroDbContext>>()
            .InstancePerLifetimeScope();

        builder
            .RegisterType<FinanceiroDbContextAccessor>()
            .As<IEfDbContextAccessor<FinanceiroDbContext>>()
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
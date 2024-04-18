using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using OtelDemo.Inscricoes.HttpService.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var assemblyName = Assembly.GetExecutingAssembly().GetName();
var serviceName = assemblyName.Name;
var serviceVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString();

try
{
    Log.ForContext("ApplicationName", serviceName).Information("Starting application");
    builder.Services
        .AddLogs(builder.Configuration, serviceName!)
        .AddTelemetry(serviceName!, serviceVersion!, builder.Configuration)
        .AddEndpointsApiExplorer()
        .AddSwaggerDoc()
        .AddVersioning()
        .AddCustomCors()
        .AddSecurity(builder.Configuration)
        .AddHealth(builder.Configuration)
        .AddWorkersServices(builder.Configuration)
        .AddOptions()
        .AddCaching()
        .AddMessageBroker(builder.Configuration)
        .AddTransient<TenantMiddleware>()
        .AddCustomMvc();

    builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule(new ApplicationModule());
    });
    builder.Host.UseSerilog();
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    
    var app = builder.Build();
    app.UseHealthChecks("/health-ready");
    app.UseHealthChecks("/health-check");
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseTenantDbContext();
    app.MapControllers();
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.ForContext("ApplicationName", serviceName)
        .Fatal(ex, "Program terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
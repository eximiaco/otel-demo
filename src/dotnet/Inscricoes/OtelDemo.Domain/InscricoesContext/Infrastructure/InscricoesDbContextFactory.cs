using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OtelDemo.Common.ServiceBus;
using OtelDemo.Common.Tenancy;

namespace OtelDemo.Inscricoes.InscricoesContext.Infrastructure;

public sealed class InscricoesDbContextFactory: IEFDbContextFactory<InscricoesDbContext>
{
    private readonly IConfiguration _configuration;
    private readonly IServiceBus _serviceBus;

    public InscricoesDbContextFactory(IConfiguration configuration, IServiceBus serviceBus)
    {
        _configuration = configuration;
        _serviceBus = serviceBus;
    }

    public async Task<InscricoesDbContext> CriarAsync(string codigoTenant)
    {
        var options = new DbContextOptionsBuilder<InscricoesDbContext>()
            .EnableDetailedErrors()
            //.EnableSensitiveDataLogging()                
            .UseNpgsql(_configuration.GetConnectionString("inscricoes_db"), options => options.EnableRetryOnFailure())
            //.AddRelationalTypeMappingSourcePlugin<DataTypeMappingPlugin>()
            .Options;
        return new InscricoesDbContext(options, _serviceBus);
    }
}
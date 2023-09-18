using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OtelDemo.Common.ServiceBus;
using OtelDemo.Common.UoW;

namespace OtelDemo.Inscricoes.FinanceiroContext.Infrastructure;

public sealed class FinanceiroDbContextFactory: IEfDbContextFactory<FinanceiroDbContext>
{
    private readonly IConfiguration _configuration;
    private readonly IServiceBus _serviceBus;

    public FinanceiroDbContextFactory(IConfiguration configuration, IServiceBus serviceBus)
    {
        _configuration = configuration;
        _serviceBus = serviceBus;
    }

    public async Task<FinanceiroDbContext> CriarAsync(string codigoTenant)
    {
        var options = new DbContextOptionsBuilder<FinanceiroDbContext>()
            .EnableDetailedErrors()
            //.EnableSensitiveDataLogging()                
            .UseSqlServer(_configuration.GetConnectionString("financeiro_db"), options => options.EnableRetryOnFailure())
            //.AddRelationalTypeMappingSourcePlugin<DataTypeMappingPlugin>()
            .Options;
        return new FinanceiroDbContext(options, _serviceBus);
    }
}
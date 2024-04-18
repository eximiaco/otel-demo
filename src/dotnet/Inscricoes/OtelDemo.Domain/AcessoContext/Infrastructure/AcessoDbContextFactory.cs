using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OtelDemo.Common.ServiceBus;
using OtelDemo.Common.UoW;

namespace OtelDemo.Domain.AcessoContext.Infrastructure;

public sealed class AcessoDbContextFactory: IEfDbContextFactory<AcessoDbContext>
{
    private readonly IConfiguration _configuration;
    private readonly IServiceBus _serviceBus;

    public AcessoDbContextFactory(IConfiguration configuration, IServiceBus serviceBus)
    {
        _configuration = configuration;
        _serviceBus = serviceBus;
    }

    public async Task<AcessoDbContext> CriarAsync(string codigoTenant)
    {
        var options = new DbContextOptionsBuilder<AcessoDbContext>()
            .EnableDetailedErrors()
            //.EnableSensitiveDataLogging()                
            .UseSqlServer(_configuration.GetConnectionString("financeiro_db"), options => options.EnableRetryOnFailure())
            //.AddRelationalTypeMappingSourcePlugin<DataTypeMappingPlugin>()
            .Options;
        return new AcessoDbContext(options, _serviceBus);
    }
}
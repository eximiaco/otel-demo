using OtelDemo.Common.UoW;
using OtelDemo.Domain.AcessoContext.Infrastructure;

namespace OtelDemo.Acesso.BrokerConsumer;

public class TenantMiddleware : IMiddleware
{
    private readonly IEfDbContextFactory<AcessoDbContext> _factory;
    private readonly IEfDbContextAccessor<AcessoDbContext> _accessor;

    public TenantMiddleware(
        IEfDbContextFactory<AcessoDbContext> factory,
         IEfDbContextAccessor<AcessoDbContext> accessor)
    {
        _factory = factory;
        _accessor = accessor;
    }

    // public async Task InvokeAsync(HttpContext context)
    // {
    //     
    // }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        using (var contexto = await _factory.CriarAsync(""))
        {
            _accessor.Register(contexto);
            // Call the next delegate/middleware in the pipeline.
            await next(context);
            _accessor.Clear();    
        }
    }
}

public static class TenantMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantDbContext(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TenantMiddleware>();
    }
}
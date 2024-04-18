using OtelDemo.Common.UoW;
using OtelDemo.Domain.InscricoesContext.Infrastructure;
using OtelDemo.Inscricoes.FinanceiroContext.Infrastructure;

namespace OtelDemo.Financeiro.HttpService.Infrastructure;

public class TenantMiddleware : IMiddleware
{
    private readonly IEfDbContextFactory<FinanceiroDbContext> _factory;
    private readonly IEfDbContextAccessor<FinanceiroDbContext> _accessor;

    public TenantMiddleware(
        IEfDbContextFactory<FinanceiroDbContext> factory,
         IEfDbContextAccessor<FinanceiroDbContext> accessor)
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
        using (var contexto = await _factory.CriarAsync(context.Request.Headers["x-tenant"]))
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
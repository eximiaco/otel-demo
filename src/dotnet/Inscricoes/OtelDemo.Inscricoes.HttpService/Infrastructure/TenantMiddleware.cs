using OtelDemo.Common.Tenancy;
using OtelDemo.Inscricoes.Domain.Infrastructure;

namespace OtelDemo.Inscricoes.HttpService.Infrastructure;

public class TenantMiddleware : IMiddleware
{
    private readonly IEFDbContextFactory<InscricoesDbContext> _factory;
    private readonly IEFDbContextAccessor<InscricoesDbContext> _accessor;

    public TenantMiddleware(
        IEFDbContextFactory<InscricoesDbContext> factory,
         IEFDbContextAccessor<InscricoesDbContext> accessor)
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
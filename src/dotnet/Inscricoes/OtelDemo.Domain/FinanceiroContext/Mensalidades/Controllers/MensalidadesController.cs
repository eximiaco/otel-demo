using Microsoft.AspNetCore.Mvc;
using OtelDemo.Domain.FinanceiroContext.Mensalidades.Aplicacao;
using OtelDemo.Domain.FinanceiroContext.Mensalidades.Comandos;

namespace OtelDemo.Domain.FinanceiroContext.Mensalidades.Controllers;

[ApiController]
//[Authorize()]
[Route("api/v{version:apiVersion}/{controller}")]
[ApiVersion("1.0")]
public sealed class MensalidadesController : ControllerBase
{

    public MensalidadesController()
    {
    }
    
    [HttpPost("Inscricoes/{id}/Adicional")]
    public async Task<IActionResult> GerarAdicional(
        [FromServices] GerarAdicionalParaIncricaoHandler handler,
        string id,
        CancellationToken cancellationToken)
    {
        if(!Guid.TryParse(id, out Guid guidId))
            return NotFound();
        var result = await handler.Executar(new GerarAdicionalParaIncricaoComando(guidId), cancellationToken);
        if (result.IsFailure)
            return BadRequest();
        return Ok();
    }
}
using Microsoft.AspNetCore.Mvc;
using OtelDemo.Domain.InscricoesContext.Inscricoes.Comandos;
using OtelDemo.Domain.InscricoesContext.QueryModel;

namespace OtelDemo.Domain.InscricoesContext.Inscricoes.Controllers;

[ApiController]
//[Authorize()]
[Route("api/v{version:apiVersion}/{controller}")]
[ApiVersion("1.0")]
public sealed class InscricoesController : ControllerBase
{
    private readonly RealizarInscricaoHandler _realizarInscricaoHandler;

    public InscricoesController()
    {
        
    }
    
    public record NovaInscricaoModel(string CpfAluno, string CpfResponsavel, int CodigoTurma);
    
    [HttpPost]
    public async Task<IActionResult> RealizarInscricao(
        [FromServices]RealizarInscricaoHandler realizarInscricaoHandler,
        [FromBody]NovaInscricaoModel input, 
        CancellationToken cancellationToken)
    {
        var comando = RealizarInscricaoComando.Criar(
            input.CpfAluno, 
            input.CpfResponsavel, 
            input.CodigoTurma);
        if (comando.IsFailure)
            return BadRequest(comando.Error);

        var resultado = await _realizarInscricaoHandler.Executar(comando.Value, cancellationToken);
        if (resultado.IsFailure)
            return BadRequest(resultado.Error);

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ConsultarInscricao(
        [FromServices] InscricoesQuery inscricoesQuery,
        string id, 
        CancellationToken cancellationToken)
    {
        if(!Guid.TryParse(id, out Guid guidId))
            return NotFound();
        var inscricao = await inscricoesQuery.Get(guidId, cancellationToken);
        if (inscricao == null)
            return NotFound();
        return Ok(inscricao);
    }
    
}
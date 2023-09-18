using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OtelDemo.Inscricoes.InscricoesContext.Inscricoes.Comandos;

namespace OtelDemo.Inscricoes.HttpService.Controllers;

[ApiController]
//[Authorize()]
[Route("api/v{version:apiVersion}/{controller}")]
[ApiVersion("1.0")]
public sealed class InscricoesController : ControllerBase
{
    private readonly RealizarInscricaoHandler _realizarInscricaoHandler;

    public InscricoesController(RealizarInscricaoHandler realizarInscricaoHandler)
    {
        _realizarInscricaoHandler = realizarInscricaoHandler;
    }
    
    public record NovaInscricaoModel(string CpfAluno, string CpfResponsavel, int CodigoTurma);
    
    [HttpPost]
    public async Task<IActionResult> RealizarInscricao(
        [FromBody]NovaInscricaoModel input, CancellationToken cancellationToken)
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
}
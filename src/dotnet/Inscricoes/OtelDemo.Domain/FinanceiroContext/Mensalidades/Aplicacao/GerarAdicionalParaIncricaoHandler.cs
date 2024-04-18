using CSharpFunctionalExtensions;
using Flurl;
using Flurl.Http;
using OtelDemo.Common;
using OtelDemo.Domain.FinanceiroContext.Mensalidades.Comandos;

namespace OtelDemo.Domain.FinanceiroContext.Mensalidades.Aplicacao;

public class GerarAdicionalParaIncricaoHandler : IService<GerarAdicionalParaIncricaoHandler>
{
    public async Task<Result> Executar(GerarAdicionalParaIncricaoComando comando, CancellationToken cancellationToken)
    {
        var uri = "https://localhost:7149/api/v1";
        var inscricao = await uri
            .AppendPathSegment("Inscricoes")
            .AppendPathSegment(comando.InscricaoId)
            .GetJsonAsync<InscricaoSummayViewModel>(cancellationToken: cancellationToken);
        return Result.Success();
    }
    
    public record InscricaoSummayViewModel(Guid Id, bool Ativa);
}
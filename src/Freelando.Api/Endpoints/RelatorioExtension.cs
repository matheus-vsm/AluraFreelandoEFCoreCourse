using Freelando.Api.Converters;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints
{
    public static class RelatorioExtension
    {
        public static void AddEndPointRelatorio(this WebApplication app)
        {
            app.MapGet("/relatorios/precoContrato", ([FromServices] FreelandoContext contexto) =>
            {
                var consulta = contexto.Contratos.Where(c => c.Valor > 1000).ToList();

                return consulta;
            }).WithTags("Relatórios").WithOpenApi();
        }
    }
}

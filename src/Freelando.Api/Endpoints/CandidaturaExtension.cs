using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class CandidaturaExtension
{
    public static void AddEndPointCandidatura(this WebApplication app)
    {
        app.MapGet("/candidaturas", async ([FromServices] CandidaturaConverter converter, [FromServices] FreelandoContext contexto) =>
        {
            // .AsNoTracking() → melhora performance, porque você só quer ler (não vai alterar).
            var candidatura = converter.EntityListToResponseList(contexto.Candidaturas.AsNoTracking().ToList());

            // ChangeTracker do EF Core rastreia mudanças feitas nas entidades.
            var entries = contexto.ChangeTracker.Entries();

            return Results.Ok(await Task.FromResult(candidatura));
        }).WithTags("Candidatura").WithOpenApi();
        // .WithTags("Candidatura") → organiza endpoints por categoria no Swagger.
        // .WithOpenApi() → expõe a rota corretamente na documentação OpenAPI / Swagger.

        app.MapPost("/candidatura", async ([FromServices] CandidaturaConverter converter, [FromServices] FreelandoContext contexto, CandidaturaRequest candidaturaRequest) =>
        {
            var candidatura = converter.RequestToEntity(candidaturaRequest);
            await contexto.Candidaturas.AddAsync(candidatura);
            await contexto.SaveChangesAsync();

            return Results.Created($"/candidatura/{candidatura.Id}", candidatura);
        }).WithTags("Candidatura").WithOpenApi();
    }
}

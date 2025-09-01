using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Freelando.Modelo;
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

        app.MapPut("/candidatura/{id}", async ([FromServices] CandidaturaConverter converter, [FromServices] FreelandoContext contexto, Guid id, CandidaturaRequest candidaturaRequest) =>
        {
            var candidatura = await contexto.Candidaturas.FindAsync(id);
            if (candidatura is null) return Results.NotFound();
            
            var candidaturaAtualizada = converter.RequestToEntity(candidaturaRequest);
            candidatura.ValorProposto = candidaturaAtualizada.ValorProposto;
            candidatura.DescricaoProposta = candidaturaAtualizada.DescricaoProposta;
            candidatura.DuracaoProposta = candidaturaAtualizada.DuracaoProposta;
            candidatura.Status = candidaturaAtualizada.Status;
            await contexto.SaveChangesAsync();

            return Results.Ok(candidatura);
        }).WithTags("Candidatura").WithOpenApi();
    }
}

﻿using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ContratoExtension
{
    public static void AddEndPointContrato(this WebApplication app)
    {
        app.MapGet("/contratos", async ([FromServices] ContratoConverter converter, [FromServices] FreelandoContext contexto) =>
        {
            var contrato = converter.EntityListToResponseList(contexto.Contratos.AsNoTracking().ToList());
            var entries = contexto.ChangeTracker.Entries();
            return Results.Ok(await Task.FromResult(contrato));
        }).WithTags("Contrato").WithOpenApi();

        app.MapPost("/contrato", async ([FromServices] ContratoConverter converter, [FromServices] FreelandoContext contexto, ContratoRequest contratoRequest) =>
        {
            using var transaction = await contexto.Database.BeginTransactionAsync();
            try
            {
                //ponto de salvamento ou CheckPoint
                transaction.CreateSavepoint("Savepoint");

                var contrato = converter.RequestToEntity(contratoRequest);
                await contexto.Contratos.AddAsync(contrato);
                await contexto.SaveChangesAsync();

                await transaction.CommitAsync();

                return Results.Created($"/contrato/{contrato.Id}", contrato);
            }
            catch (DbUpdateConcurrencyException e)
            {
                return Results.BadRequest($"Problemas de Simultaneidade {e.Message}.");
            }
            catch (Exception e)
            {
                //caso de algum problema na transação, ele retorna para o CheckPoint criado
                transaction.RollbackToSavepoint("Savepoint");
                return Results.BadRequest(e.Message);
            }
        }).WithTags("Contrato").WithOpenApi();

        app.MapPut("/contrato{id}", async ([FromServices] ContratoConverter converter, [FromServices] FreelandoContext contexto, ContratoRequest contratoRequest, Guid id) =>
        {
            var contrato = await contexto.Contratos.FindAsync(id);
            if (contrato is null) return Results.NotFound();

            var contratoAtualizado = converter.RequestToEntity(contratoRequest);
            contrato.Valor = contratoAtualizado.Valor;
            contrato.Vigencia = contratoAtualizado.Vigencia;
            await contexto.SaveChangesAsync();

            return Results.Ok(contrato);
        }).WithTags("Contrato").WithOpenApi();

        app.MapDelete("/contrato/{id}", async ([FromServices] FreelandoContext contexto, Guid id) =>
        {
            var contrato = await contexto.Contratos.FindAsync(id);
            if (contrato is null) return Results.NotFound();

            contexto.Contratos.Remove(contrato);
            await contexto.SaveChangesAsync();

            return Results.NoContent();
        }).WithTags("Contrato").WithOpenApi();
    }
}

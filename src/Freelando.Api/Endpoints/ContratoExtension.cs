using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelando.Api.Endpoints;

public static class ContratoExtension
{
    public static void AddEndPointContrato(this WebApplication app)
    {
        app.MapGet("/contratos", async ([FromServices] ContratoConverter converter, [FromServices] IUnitOfWork unitOfWork) =>
        {
            var contrato = converter.EntityListToResponseList(await unitOfWork.ContratoRepository.BuscarTodos());

            return Results.Ok(await Task.FromResult(contrato));
        }).WithTags("Contrato").WithOpenApi();

        app.MapPost("/contrato", async ([FromServices] ContratoConverter converter, [FromServices] IUnitOfWork unitOfWork, ContratoRequest contratoRequest) =>
        {
            using var transaction = await unitOfWork.contexto.Database.BeginTransactionAsync();
            try
            {
                //ponto de salvamento ou CheckPoint
                transaction.CreateSavepoint("Savepoint");

                var contrato = converter.RequestToEntity(contratoRequest);
                await unitOfWork.contexto.Contratos.AddAsync(contrato);
                await unitOfWork.contexto.SaveChangesAsync();

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

        app.MapPut("/contrato{id}", async ([FromServices] ContratoConverter converter, [FromServices] IUnitOfWork unitOfWork, ContratoRequest contratoRequest, Guid id) =>
        {
            var contrato = await unitOfWork.ContratoRepository.BuscarPorId(x => x.Id == id);
            if (contrato is null) return Results.NotFound();

            var contratoAtualizado = converter.RequestToEntity(contratoRequest);
            contrato.Valor = contratoAtualizado.Valor;
            contrato.Vigencia = contratoAtualizado.Vigencia;

            await unitOfWork.ContratoRepository.Atualizar(contrato);
            await unitOfWork.Commit();

            return Results.Ok(contrato);
        }).WithTags("Contrato").WithOpenApi();

        app.MapDelete("/contrato/{id}", async ([FromServices] IUnitOfWork unitOfWork, Guid id) =>
        {
            var contrato = await unitOfWork.ContratoRepository.BuscarPorId(x => x.Id == id);
            if (contrato is null) return Results.NotFound();

            await unitOfWork.ContratoRepository.Deletar(contrato);
            await unitOfWork.Commit();

            return Results.NoContent();
        }).WithTags("Contrato").WithOpenApi();
    }
}

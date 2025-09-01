using Freelando.Api.Converters;
using Freelando.Api.Requests;
using Freelando.Dados;
using Freelando.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using System.Transactions;
namespace Freelando.Api.Endpoints;

public static class EspecialidadeExtension
{
    public static void AddEndPointEspecialidade(this WebApplication app)
    {
        app.MapGet("/especialidades", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext contexto) =>
        {
            var especialidades = converter.EntityListToResponseList(contexto.Especialidades.ToList());

            return Results.Ok(await Task.FromResult(especialidades));
        }).WithTags("Especialidade").WithOpenApi();

        app.MapPost("/especialidade", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext contexto, EspecialidadeRequest especialidadeRequest) =>
        {
            var especialidade = converter.RequestToEntity(especialidadeRequest);

            //verifica se a descrição é vazia e se a primeira letra é maiuscula
            Func<Especialidade, bool> validarDescricao = especialidade => 
            !string.IsNullOrEmpty(especialidade.Descricao) && 
            char.IsUpper(especialidade.Descricao[0]);

            if (!validarDescricao(especialidade)) return Results.BadRequest("A Descrição não pode estar em Branco e deve começar com Letra Maiúscula!");

            await contexto.Especialidades.AddAsync(especialidade);
            await contexto.SaveChangesAsync();

            return Results.Created($"/especialidade/{especialidade.Id}", especialidade);
            //O Created é usado quando você cria um novo recurso em uma API. Ele deixa explícito para o cliente “foi criado”, “aqui está o recurso” e “este é o endereço dele”.
        }).WithTags("Especialidade").WithOpenApi();

        app.MapPut("/especialidade/{id}", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext contexto, Guid id, EspecialidadeRequest especialidadeRequest) =>
        {
            var especialidade = await contexto.Especialidades.FindAsync(id);
            if (especialidade is null) return Results.NotFound();

            var especialidadeAtualizada = converter.RequestToEntity(especialidadeRequest);
            especialidade.Descricao = especialidadeAtualizada.Descricao;
            especialidade.Projetos = especialidadeAtualizada.Projetos;
            await contexto.SaveChangesAsync();

            return Results.Ok(especialidade);
        }).WithTags("Especialidade").WithOpenApi();

        //app.MapDelete("/especialidade/{id}", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext contexto, Guid id) =>
        //{
        //    var especialidade = await contexto.Especialidades.FindAsync(id);
        //    if (especialidade is null) return Results.NotFound();

        //    contexto.Especialidades.Remove(especialidade);
        //    await contexto.SaveChangesAsync();

        //    return Results.NoContent();
        //    //Retorna um 204 No Content, que é o código HTTP padrão quando algo foi excluído com sucesso e não tem nada para retornar no corpo da resposta.
        //}).WithTags("Especialidade").WithOpenApi();
        app.MapDelete("/especialidade/{id}", async ([FromServices] EspecialidadeConverter converter, [FromServices] FreelandoContext contexto, Guid id) =>
        {
            using (var transaction = contexto.Database.BeginTransaction())
            {
                try
                {
                    var especialidade = await contexto.Especialidades.FindAsync(id);
                    if (especialidade is null) return Results.NotFound();

                    contexto.Especialidades.Remove(especialidade);
                    await contexto.SaveChangesAsync();
                    transaction.Commit();

                    return Results.NoContent();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }            
        }).WithTags("Especialidade").WithOpenApi();
    }
}

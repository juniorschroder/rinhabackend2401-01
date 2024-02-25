using Microsoft.AspNetCore.Mvc;
using RinhaBackend.Domain.Abstractions.Services;
using RinhaBackend.Domain.Entities;
using RinhaBackend.Domain.Exceptions;

namespace RinhaBackend.Api.Endpoints;

public static class ClientesModule
{
    public static void AddClientesEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/clientes/{id}/extrato", async Task<IResult>(int id, IExtratoService extratoService) =>
        {
            try
            {
                var extrato = await extratoService.RetornaExtratoDoClienteAsync(id);
                return TypedResults.Ok(extrato);
            }
            catch (ClienteNaoEncontradoException)
            {
                return Results.NotFound();
            }
        });
        routeBuilder.MapPost("/clientes/{id}/transacoes",
            async Task<IResult>(int id, [FromBody]RegistrarTransacao? registrarTransacao, ITransacaoService transacaoService) =>
            {
                try
                {
                    var retorno = await transacaoService.RegistrarTransacaoAsync(id, registrarTransacao);
                    return Results.Ok(retorno);
                }
                catch (ClienteNaoEncontradoException)
                {
                    return Results.NotFound();
                }
                catch (Exception ex) when (ex is LimiteExcedidoException or DadosIncorretosException)
                {
                    return Results.UnprocessableEntity();
                }
            });
    }
}
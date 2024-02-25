using RinhaBackend.Domain.DTO;
using RinhaBackend.Domain.Entities;

namespace RinhaBackend.Domain.Abstractions.Services;

public interface ITransacaoService
{
    Task<RetornoTransacao> RegistrarTransacaoAsync(int clienteId, RegistrarTransacao? dados);
}
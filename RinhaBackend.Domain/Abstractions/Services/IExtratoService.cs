using RinhaBackend.Domain.DTO;

namespace RinhaBackend.Domain.Abstractions.Services;

public interface IExtratoService
{
    Task<Extrato> RetornaExtratoDoClienteAsync(int clienteId);
}
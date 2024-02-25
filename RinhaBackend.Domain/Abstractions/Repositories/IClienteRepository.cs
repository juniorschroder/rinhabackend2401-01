using RinhaBackend.Domain.Entities;

namespace RinhaBackend.Domain.Abstractions.Repositories;

public interface IClienteRepository : IRepositoryBase<Cliente, int>
{
    Task<int> RetornaSaldoClienteAsync(int clienteId);
    Task<Cliente> RetonarClienteAsync(int clienteId);
}
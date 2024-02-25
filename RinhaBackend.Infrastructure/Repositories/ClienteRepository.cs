using Microsoft.EntityFrameworkCore;
using RinhaBackend.Domain.Abstractions.Repositories;
using RinhaBackend.Domain.Entities;
using RinhaBackend.Domain.Exceptions;
using RinhaBackend.Infrastructure.Data;

namespace RinhaBackend.Infrastructure.Repositories;

public class ClienteRepository : RepositoryBase<Cliente, int>, IClienteRepository
{
    public ClienteRepository(RinhaContext context) : base(context) { }
    public async Task<int> RetornaSaldoClienteAsync(int clienteId)
    {
        var retorno = await Context.Database
            .SqlQuery<int>($"select t.saldo from transacao t where t.cliente_id = {clienteId} order by t.id desc limit 1")
            .ToListAsync();
        return retorno.FirstOrDefault();
    }

    public async Task<Cliente> RetonarClienteAsync(int clienteId)
    {
        var cliente = await GetByIdAsync(clienteId);
        if (cliente == null) throw new ClienteNaoEncontradoException();
        return cliente;
    }
}
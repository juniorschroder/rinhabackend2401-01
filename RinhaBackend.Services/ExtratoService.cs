using RinhaBackend.Domain.Abstractions.Repositories;
using RinhaBackend.Domain.Abstractions.Services;
using RinhaBackend.Domain.DTO;

namespace RinhaBackend.Services;

public class ExtratoService : IExtratoService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly ITransacaoRepository _transacaoRepository;

    public ExtratoService(IClienteRepository clienteRepository, ITransacaoRepository transacaoRepository)
    {
        _clienteRepository = clienteRepository;
        _transacaoRepository = transacaoRepository;
    }

    public async Task<Extrato> RetornaExtratoDoClienteAsync(int clienteId)
    {
        var cliente = await _clienteRepository.RetonarClienteAsync(clienteId);
        var saldoCliente = await _clienteRepository.RetornaSaldoClienteAsync(clienteId);
        return new Extrato
        {
            Saldo = new Saldo
            {
                Limite = cliente.Limite, Total = saldoCliente, 
                DataExtrato = DateTime.Now
            },
            UltimasTransacoes = await _transacaoRepository
                .FindAsync(x => x.ClienteId == clienteId, 10, o => o.Id)
        };
    }
}
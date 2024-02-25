using RinhaBackend.Domain.Abstractions.Repositories;
using RinhaBackend.Domain.Abstractions.Services;
using RinhaBackend.Domain.DTO;
using RinhaBackend.Domain.Entities;
using RinhaBackend.Domain.Exceptions;

namespace RinhaBackend.Services;

public class TransacaoService : ITransacaoService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly ITransacaoRepository _transacaoRepository;

    public TransacaoService(IClienteRepository clienteRepository, ITransacaoRepository transacaoRepository)
    {
        _clienteRepository = clienteRepository;
        _transacaoRepository = transacaoRepository;
    }

    public async Task<RetornoTransacao> RegistrarTransacaoAsync(int clienteId, RegistrarTransacao? dados)
    {
        var cliente = await _clienteRepository.RetonarClienteAsync(clienteId);
        ValidarDados(dados);
        var saldo = await VerificaSaldoAtualizadoAsync(cliente, dados!.Valor, dados.Tipo);
        var transacao = new Transacao
        {
            Cliente = cliente, Descricao = dados.Descricao,
            Tipo = dados.Tipo, Valor = dados.Valor, RealizadaEm = DateTime.UtcNow
        };
        await _transacaoRepository.AddAsync(transacao);
        return new RetornoTransacao(cliente.Limite, saldo);
    }

    private void ValidarDados(RegistrarTransacao? dados)
    {
        if (dados == null) throw new DadosIncorretosException();
        dados.Validar();
    }

    private async Task<int> VerificaSaldoAtualizadoAsync(Cliente cliente, int valor, char tipo)
    {
        var saldoAtualizado = await _clienteRepository.RetornaSaldoClienteAsync(cliente.Id);
        if (tipo == 'c')
            saldoAtualizado += valor;
        else
            saldoAtualizado -= valor;

        if (saldoAtualizado < (cliente.Limite - (cliente.Limite * 2)))
            throw new LimiteExcedidoException();

        return saldoAtualizado;
    }
}
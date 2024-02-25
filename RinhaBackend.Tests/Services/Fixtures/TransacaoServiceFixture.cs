using Moq;
using RinhaBackend.Domain.Abstractions.Repositories;
using RinhaBackend.Domain.Entities;
using RinhaBackend.Services;

namespace RinhaBackend.Tests.Services.Fixtures;

public class TransacaoServiceFixture
{
    public const int IdCliente = 1;
    
    public Mock<ITransacaoRepository> TransacaoRepositoryMock;

    public TransacaoServiceFixture()
    {
        TransacaoRepositoryMock = new Mock<ITransacaoRepository>();
    }

    public TransacaoService RetornaTransacaoService() => new(ObtemClienteRepository(), TransacaoRepositoryMock.Object);

    private IClienteRepository ObtemClienteRepository()
    {
        var mock = new Mock<IClienteRepository>();
        mock.Setup(x => x.RetonarClienteAsync(IdCliente)).ReturnsAsync(new Cliente
        {
            Id = IdCliente, Limite = 1000
        });
        mock.Setup(x => x.RetornaSaldoClienteAsync(IdCliente)).ReturnsAsync(100);
        return mock.Object;
    }
}
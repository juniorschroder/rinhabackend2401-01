using FluentAssertions;
using Moq;
using RinhaBackend.Domain.Entities;
using RinhaBackend.Domain.Exceptions;
using RinhaBackend.Services;
using RinhaBackend.Tests.Services.Fixtures;

namespace RinhaBackend.Tests.Services;

public class TransacaoServiceTest : IClassFixture<TransacaoServiceFixture>
{
    private readonly TransacaoService _transacaoService;
    private readonly TransacaoServiceFixture _fixture;
    
    public TransacaoServiceTest(TransacaoServiceFixture fixture)
    {
        _fixture = fixture;
        _transacaoService = fixture.RetornaTransacaoService();
    }

    [Fact]
    public async Task Deve_Retornar_Erro_Quando_O_Limite_Do_Cliente_For_Insuficiente()
    {
        // Arrange
        var dadosTransacao = new RegistrarTransacao
        {
            Descricao = "Deposito 1", Tipo = 'd', Valor = 1500
        };
        
        // Act
        Func<Task> registrarTransacao = () => _transacaoService
            .RegistrarTransacaoAsync(TransacaoServiceFixture.IdCliente, dadosTransacao);

        // Assert
        await registrarTransacao.Should().ThrowAsync<LimiteExcedidoException>();
    }
    
    [Theory]
    [InlineData(null, 'd', 100)]
    [InlineData("Deposito", 'x', 100)]
    [InlineData("Deposito", 'c', 0)]
    [InlineData("Descriçao de uma transaçao de Deposito", 'c', 100)]
    public async Task Deve_Retornar_Erro_Quando_Os_Dados_Invalidos(string descricao, char tipoTransacao, int valor)
    {
        // Arrange
        var dadosTransacao = new RegistrarTransacao
        {
            Descricao = descricao, Tipo = tipoTransacao, Valor = valor
        };
        
        // Act
        Func<Task> registrarTransacao = () => _transacaoService
            .RegistrarTransacaoAsync(TransacaoServiceFixture.IdCliente, dadosTransacao);

        // Assert
        await registrarTransacao.Should().ThrowAsync<DadosIncorretosException>();
    }
    
    [Fact]
    public async Task Deve_Registrar_Transacao_Com_Sucesso()
    {
        // Arrange
        var dadosTransacao = new RegistrarTransacao
        {
            Descricao = "Deposito 1", Tipo = 'c', Valor = 500
        };
        
        // Act
        await _transacaoService
            .RegistrarTransacaoAsync(TransacaoServiceFixture.IdCliente, dadosTransacao);

        // Assert
        _fixture.TransacaoRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Transacao>()), Times.Once);
    }
}
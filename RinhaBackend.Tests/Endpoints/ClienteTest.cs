using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using RinhaBackend.Domain.DTO;
using RinhaBackend.Domain.Entities;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RinhaBackend.Tests.Endpoints;

public class ClienteTest
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public ClienteTest()
    {
        var applicationFactory = new RinhaBackendApiFactory();
        _client = applicationFactory.CreateClient();
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
    }

    [Fact]
    public async Task Deve_Retornar_Extrato_Do_Cliente()
    {
        // Arrange
        var urlConsultaSaldo = "/clientes/1/extrato";
        
        // Act
        var retorno = await _client.GetAsync(urlConsultaSaldo);

        // Assert
        retorno.EnsureSuccessStatusCode();
        var conteudo = await retorno.Content.ReadAsStringAsync();
        var extrato = JsonSerializer.Deserialize<Extrato>(conteudo, _jsonSerializerOptions);
        extrato.UltimasTransacoes.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task Deve_Retornar_Erro_Ao_Consultar_Extrato_Quando_Cliente_Invalido()
    {
        // Arrange
        var urlConsultaSaldo = "/clientes/111/extrato";
        
        // Act
        var retorno = await _client.GetAsync(urlConsultaSaldo);

        // Assert
        retorno.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(5, 'c', 500000, 500000, 500000)]
    [InlineData(2, 'd', 100, 80000, -100)]
    public async Task Deve_Registrar_Transacao_Com_Sucesso(int clienteId, char tipoTransacao, 
        int valorTransacao, int limiteEsperado, int saldoEsperado)
    {
        // Arrange
        var urlTransacao = $"/clientes/{clienteId}/transacoes";
        var dados = RetornarStringContentInputTransacao("Transacao", tipoTransacao, valorTransacao);

        // Act
        var retorno = await _client.PostAsync(urlTransacao, dados);

        // Assert
        retorno.EnsureSuccessStatusCode();
        var conteudo = await retorno.Content.ReadAsStringAsync();
        var retornoTransacao = JsonSerializer.Deserialize<RetornoTransacao>(conteudo, _jsonSerializerOptions);
        retornoTransacao.Limite.Should().Be(limiteEsperado);
        retornoTransacao.Saldo.Should().Be(saldoEsperado);
    }
    
    [Fact]
    public async Task Deve_Retornar_Erro_Ao_Registrar_Transacao_Para_Cliente_Inexistente()
    {
        // Arrange
        var urlTransacao = "/clientes/111/transacoes";
        var dados = RetornarStringContentInputTransacao("Transacao", 'c', 11300);

        // Act
        var retorno = await _client.PostAsync(urlTransacao, dados);

        // Assert
        retorno.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(null, 'c', 1000)]
    [InlineData("Transacao", 'x', 1000)]
    [InlineData("Transacao", 'c', 0)]
    public async Task Deve_Retornar_Erro_Ao_Registrar_Transacao_Informando_Dados_Invalidos(string descricao, 
        char tipoTransacao, int valor)
    {
        // Arrange
        var urlTransacao = "/clientes/3/transacoes";
        var dados = RetornarStringContentInputTransacao(descricao, tipoTransacao, valor);

        // Act
        var retorno = await _client.PostAsync(urlTransacao, dados);

        // Assert
        retorno.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }
    
    private StringContent RetornarStringContentInputTransacao(string descricao, char tipoTransacao, int valor) => 
        new(JsonSerializer.Serialize(new RegistrarTransacao
        {
            Descricao = descricao, Tipo = tipoTransacao, Valor = valor
        }), Encoding.UTF8, "application/json");
}
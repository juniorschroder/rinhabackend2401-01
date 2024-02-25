using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RinhaBackend.Domain.Entities;
using RinhaBackend.Infrastructure.Data;

namespace RinhaBackend.Tests;

internal class RinhaBackendApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<RinhaContext>));
            var connection = new SqliteConnection("datasource=:memory:");
            connection.Open();
            services.AddDbContextPool<RinhaContext>(options =>
            {
                options.UseSqlite(connection);
            });
            var dbContext = CriarDbContext(services);
            if (dbContext.Database.EnsureCreated())
                PopularBaseDeDados(dbContext);
        });
    }

    private void PopularBaseDeDados(RinhaContext dbContext)
    {
        using var viewCommand = dbContext.Database.GetDbConnection().CreateCommand();
        viewCommand.CommandText = RetornaComandoSqlCriacaoTriggerAtualizacaoSaldo();
        viewCommand.ExecuteNonQuery();
        var clientes = new List<Cliente>
        {
            new() { Id = 1, Limite = 100000 },
            new() { Id = 2, Limite = 80000 },
            new() { Id = 3, Limite = 1000000 },
            new() { Id = 4, Limite = 10000000 },
            new() { Id = 5, Limite = 500000 }
        };
        var transacoes = new List<Transacao>()
        {
            new()
            {
                Cliente = clientes.First(), Descricao = "Deposito 1",
                Tipo = 'c', Valor = 1000, RealizadaEm = DateTime.Now
            }
            ,
            new()
            {
                Cliente = clientes.First(), Descricao = "Saque 1",
                Tipo = 'c', Valor = 100, RealizadaEm = DateTime.Now
            }
        };
        dbContext.AddRange(clientes);
        dbContext.AddRange(transacoes);
        dbContext.SaveChanges();
    }

    private string RetornaComandoSqlCriacaoTriggerAtualizacaoSaldo()
    {
        return @"
        CREATE TRIGGER tbi_atualiza_saldo_cliente_credito
AFTER INSERT ON transacao 
WHEN NEW.tipo_transacao = 'c'
BEGIN
    -- Atualiza o saldo com base no tipo de transação
	UPDATE transacao
SET saldo = (
    SELECT COALESCE(t2.saldo, 0)+NEW.valor
    FROM transacao AS t2
    WHERE t2.cliente_id = NEW.cliente_id AND t2.id < NEW.id
    ORDER BY t2.id DESC
    LIMIT 1
) WHERE id = NEW.id;
END;

CREATE TRIGGER tbi_atualiza_saldo_cliente_debito
AFTER INSERT ON transacao 
WHEN NEW.tipo_transacao = 'd'
BEGIN
    -- Atualiza o saldo com base no tipo de transação
	UPDATE transacao
SET saldo = (
    SELECT COALESCE(t2.saldo, 0)-NEW.valor
    FROM transacao AS t2
    WHERE t2.cliente_id = NEW.cliente_id AND t2.id < NEW.id
    ORDER BY t2.id DESC
    LIMIT 1
) WHERE id = NEW.id;
END;";
    }

    private static RinhaContext CriarDbContext(IServiceCollection serviceCollection)
    {
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<RinhaContext>();
        return dbContext;
    }
}
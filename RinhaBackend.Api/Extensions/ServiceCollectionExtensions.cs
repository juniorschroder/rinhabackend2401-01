using RinhaBackend.Domain.Abstractions.Repositories;
using RinhaBackend.Domain.Abstractions.Services;
using RinhaBackend.Infrastructure.Repositories;
using RinhaBackend.Services;

namespace RinhaBackend.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IExtratoService, ExtratoService>();
        serviceCollection.AddScoped<ITransacaoService, TransacaoService>();
    }
    
    public static void AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IClienteRepository, ClienteRepository>();
        serviceCollection.AddScoped<ITransacaoRepository, TransacaoRepository>();
    }
}
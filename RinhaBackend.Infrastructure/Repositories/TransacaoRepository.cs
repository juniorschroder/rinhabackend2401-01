using RinhaBackend.Domain.Abstractions.Repositories;
using RinhaBackend.Domain.Entities;
using RinhaBackend.Infrastructure.Data;

namespace RinhaBackend.Infrastructure.Repositories;

public class TransacaoRepository : RepositoryBase<Transacao, int>, ITransacaoRepository
{
    public TransacaoRepository(RinhaContext context) : base(context) { }
}
using RinhaBackend.Domain.Entities;

namespace RinhaBackend.Domain.DTO;

public class Extrato
{
    public Saldo Saldo { get; set; } = new();
    public IEnumerable<Transacao> UltimasTransacoes { get; set; } = new List<Transacao>();
}
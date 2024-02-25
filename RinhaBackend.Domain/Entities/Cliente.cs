namespace RinhaBackend.Domain.Entities;

public class Cliente
{
    public int Id { get; set; }
    public int Limite { get; set; }

    public virtual ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
}
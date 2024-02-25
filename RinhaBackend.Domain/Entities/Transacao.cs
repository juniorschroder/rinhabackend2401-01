using System.Text.Json.Serialization;

namespace RinhaBackend.Domain.Entities;

public class Transacao
{
    [JsonIgnore]
    public int Id { get; set; }
    [JsonIgnore]
    public int ClienteId { get; set; }
    public int Valor { get; set; }
    public char Tipo { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime RealizadaEm { get; set; }
    [JsonIgnore]
    public int? Saldo { get; }
    
    [JsonIgnore]
    public virtual Cliente Cliente { get; set; } = new();
}
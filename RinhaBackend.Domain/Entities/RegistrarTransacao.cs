using RinhaBackend.Domain.Exceptions;

namespace RinhaBackend.Domain.Entities;

public class RegistrarTransacao
{
    public int Valor { get; set; }
    public char Tipo { get; set; }
    public string Descricao { get; set; } = string.Empty;

    public void Validar()
    {
        if (!string.IsNullOrEmpty(Descricao) && Descricao.Length <= 10 
                                             && Valor > 0 && Tipo is 'c' or 'd') return;
        throw new DadosIncorretosException();
    }
}
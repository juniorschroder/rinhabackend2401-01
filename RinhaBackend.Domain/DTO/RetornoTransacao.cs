namespace RinhaBackend.Domain.DTO;

public class RetornoTransacao
{
    public RetornoTransacao(int limite, int saldo)
    {
        Limite = limite;
        Saldo = saldo;
    }
    
    public int Limite { get; set; }
    public int Saldo { get; set; }
}
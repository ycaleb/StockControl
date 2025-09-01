namespace StockControl.Models;
public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string SenhaHash { get; set; } = null!;
    public string? Cpf { get; set; }
}

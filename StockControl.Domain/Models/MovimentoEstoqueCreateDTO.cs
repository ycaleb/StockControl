namespace StockControl.Models
{
    public class MovimentoEstoqueCreateDTO
    {
        public int MaterialId { get; set; }
        public int Quantidade { get; set; }
        public string Tipo { get; set; } = string.Empty; // "entrada" ou "saida"
        public string? Observacao { get; set; }
    }
}

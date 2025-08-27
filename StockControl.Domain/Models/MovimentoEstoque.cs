namespace StockControl.Models
{
    public class MovimentoEstoque
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; } = null!;
        public DateTime Data { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
        public string Tipo { get; set; } = string.Empty; // "entrada" ou "saida"
        public string? Observacao { get; set; }
    }
}
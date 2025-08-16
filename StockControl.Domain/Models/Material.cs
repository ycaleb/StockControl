namespace ProjetoConstrucao.API.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string UnidadeMedida { get; set; } = string.Empty;
        public decimal CustoUnitario { get; set; }
        public int QuantidadeEstoque { get; set; }
    }
}
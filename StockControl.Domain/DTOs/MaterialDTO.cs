namespace StockControl.DTOs
{
    public record MaterialDTO(int Id, string Nome, string UnidadeMedida, decimal CustoUnitario, int QuantidadeEstoque);
}
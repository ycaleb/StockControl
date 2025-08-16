namespace ProjetoConstrucao.API.DTOs
{
    public record MovimentoEstoqueDTO(int MaterialId, int Quantidade, string Tipo, string? Observacao);
}
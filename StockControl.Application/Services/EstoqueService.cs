using StockControl.Application.Interfaces;
using StockControl.Models;

namespace StockControl.Application.Services;
public class EstoqueService
{
    private readonly IMaterialRepository _materialRepo;
    private readonly IMovimentoEstoqueRepository _movRepo;

    public EstoqueService(IMaterialRepository materialRepo,IMovimentoEstoqueRepository movRepo)
    {
        _materialRepo = materialRepo;
        _movRepo = movRepo;
    }

    public async Task<(bool sucesso, string mensagem, int saldo)> MovimentarAsync(MovimentoEstoqueCreate dto)
    {
        var material = await _materialRepo.GetByIdAsync(dto.MaterialId);
        if(material is null)
            return (false, "Material não encontrado", 0);

        var tipo = dto.Tipo?.Trim().ToLower();
        if(tipo is not ("entrada" or "saida"))
            return (false, "Tipo deve ser 'entrada' ou 'saida'", material.QuantidadeEstoque);

        if(tipo == "entrada")
        {
            material.QuantidadeEstoque += dto.Quantidade;
        }
        else
        {
            if(dto.Quantidade <= 0) return (false, "Quantidade inválida", material.QuantidadeEstoque);
            if(material.QuantidadeEstoque < dto.Quantidade)
                return (false, "Estoque insuficiente", material.QuantidadeEstoque);

            material.QuantidadeEstoque -= dto.Quantidade;
        }

        var movimento = new MovimentoEstoque
        {
            MaterialId = dto.MaterialId,
            Material = material,
            Quantidade = dto.Quantidade,
            ValorTotal = dto.Quantidade * material.CustoUnitario,
            Tipo = tipo!,
            Data = DateTime.UtcNow,
            Observacao = dto.Observacao
        };

        await _movRepo.AddAsync(movimento);
        await _materialRepo.UpdateAsync(material);

        return (true, "Movimento registrado", material.QuantidadeEstoque);
    }

    public async Task<List<MovimentoEstoque>> ListarMovimentosAsync(int? materialId)
        => await _movRepo.ListarAsync(materialId);
}

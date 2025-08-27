using StockControl.Models;

namespace StockControl.Application.Interfaces;
public interface IMovimentoEstoqueRepository
{
    Task AddAsync(MovimentoEstoque movimento);
    Task<List<MovimentoEstoque>> ListarAsync(int? materialId = null);
}

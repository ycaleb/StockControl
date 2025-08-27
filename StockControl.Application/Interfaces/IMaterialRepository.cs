using StockControl.Models;

namespace StockControl.Application.Interfaces;
public interface IMaterialRepository
{
    Task<List<Material>> ListAllAsync();
    Task<Material?> GetByIdAsync(int id);
    Task AddAsync(Material material);
    Task UpdateAsync(Material material);
    Task RemoveAsync(Material material);
}

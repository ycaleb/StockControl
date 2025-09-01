using StockControl.Models;

namespace StockControl.Application.Interfaces;
public interface IUsuarioRepository
{
    Task<List<Usuario>> ListAllAsync();
    Task<Usuario?> GetByIdAsync(int id);
    Task AddAsync(Usuario usuario);
    Task UpdateAsync(Usuario usuario);
    Task RemoveAsync(Usuario usuario);
}

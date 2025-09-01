using Microsoft.EntityFrameworkCore;
using StockControl.Application.Interfaces;
using StockControl.Data;
using StockControl.Models;

namespace StockControl.Infrastructure.Repositories;
public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;
    public UsuarioRepository(AppDbContext context) => _context = context;

    public async Task<List<Usuario>> ListAllAsync()
        => await _context.Usuarios.AsNoTracking().ToListAsync();

    public async Task<Usuario?> GetByIdAsync(int id)
        => await _context.Usuarios.FindAsync(id);

    public async Task AddAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Usuario usuario)
    {
        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using StockControl.Application.Interfaces;
using StockControl.Data;
using StockControl.Models;

namespace StockControl.Infrastructure.Repositories;
public class MaterialRepository : IMaterialRepository
{
    private readonly AppDbContext _context;
    public MaterialRepository(AppDbContext context) => _context = context;

    public async Task<List<Material>> ListAllAsync()
        => await _context.Materiais.AsNoTracking().ToListAsync();

    public async Task<Material?> GetByIdAsync(int id)
        => await _context.Materiais.FindAsync(id);

    public async Task AddAsync(Material material)
    {
        _context.Materiais.Add(material);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Material material)
    {
        _context.Materiais.Update(material);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Material material)
    {
        _context.Materiais.Remove(material);
        await _context.SaveChangesAsync();
    }
}

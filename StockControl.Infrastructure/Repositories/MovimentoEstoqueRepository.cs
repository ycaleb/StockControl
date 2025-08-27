using Microsoft.EntityFrameworkCore;
using StockControl.Application.Interfaces;
using StockControl.Data;
using StockControl.Models;

namespace StockControl.Infrastructure.Repositories;
public class MovimentoEstoqueRepository : IMovimentoEstoqueRepository
{
    private readonly AppDbContext _context;
    public MovimentoEstoqueRepository(AppDbContext context) => _context = context;

    public async Task AddAsync(MovimentoEstoque movimento)
    {
        _context.MovimentosEstoque.Add(movimento);
        await _context.SaveChangesAsync();
    }

    public async Task<List<MovimentoEstoque>> ListarAsync(int? materialId = null)
    {
        var query = _context.MovimentosEstoque
            .Include(m => m.Material)
            .AsNoTracking()
            .OrderByDescending(m => m.Data)
            .AsQueryable();

        if(materialId.HasValue)
            query = query.Where(m => m.MaterialId == materialId.Value);

        return await query.ToListAsync();
    }
}

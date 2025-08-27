using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockControl.Data;
using StockControl.Services;

namespace StockControl.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatoriosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RelatorioService _relatorioService;

        public RelatoriosController(AppDbContext context)
        {
            _context = context;
            _relatorioService = new RelatorioService();
        }

        // GET api/relatorios/gastos/pdf?inicio=2025-01-01&fim=2025-12-31
        [HttpGet("gastos/pdf")]
        public async Task<IActionResult> GerarRelatorioPDF([FromQuery] DateTime? inicio,[FromQuery] DateTime? fim)
        {
            var query = _context.MovimentosEstoque.Include(m => m.Material).AsQueryable();

            if(inicio.HasValue) query = query.Where(m => m.Data >= inicio.Value);
            if(fim.HasValue) query = query.Where(m => m.Data <= fim.Value);

            var movimentos = await query.OrderBy(m => m.Data).ToListAsync();

            var pdf = _relatorioService.GerarRelatorioGastos(movimentos,inicio,fim);
            return File(pdf,"application/pdf","relatorio_gastos.pdf");
        }
    }
}

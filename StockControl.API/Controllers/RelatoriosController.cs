using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockControl.Data;
using StockControl.Services;

namespace StockControl.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RelatoriosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RelatorioService _relatorioService;

        public RelatoriosController(AppDbContext context)
        {
            _context = context;
            _relatorioService = new RelatorioService();
        }

        [HttpGet("relatorio/pdf")]
        public async Task<IActionResult> GerarRelatorioPDF([FromQuery] string? inicio, [FromQuery] string? fim)
        {
            DateTime? dtInicio = null;
            DateTime? dtFim = null;

            if (!string.IsNullOrEmpty(inicio))
                dtInicio = DateTime.ParseExact(inicio, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(fim))
                dtFim = DateTime.ParseExact(fim, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            var query = _context.MovimentosEstoque.Include(m => m.Material).AsQueryable();

            if (dtInicio.HasValue) query = query.Where(m => m.Data.Date >= dtInicio.Value.Date);
            if (dtFim.HasValue) query = query.Where(m => m.Data.Date <= dtFim.Value.Date);

            var movimentos = await query.OrderBy(m => m.Data).ToListAsync();

            var pdf = _relatorioService.GerarRelatorioGastos(movimentos, dtInicio, dtFim);
            return File(pdf, "application/pdf", "relatorio_gastos.pdf");
        }


    }
}

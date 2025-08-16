using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoConstrucao.API.Data;
using ProjetoConstrucao.API.Models;

namespace ProjetoConstrucao.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstoqueController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstoqueController(AppDbContext context) => _context = context;

        [HttpPost("movimentar")]
        public async Task<ActionResult> Movimentar([FromBody] MovimentoEstoqueCreateDTO dto)
        {
            var material = await _context.Materiais.FirstOrDefaultAsync(m => m.Id == dto.MaterialId);
            if(material is null)
                return NotFound("Material não encontrado.");

            var tipo = dto.Tipo?.Trim().ToLower();
            if(tipo is not ("entrada" or "saida"))
                return BadRequest("Tipo deve ser 'entrada' ou 'saida'.");

            if(tipo == "entrada")
                material.QuantidadeEstoque += dto.Quantidade;
            else
            {
                if(dto.Quantidade <= 0) return BadRequest("Quantidade inválida.");
                if(material.QuantidadeEstoque < dto.Quantidade)
                    return BadRequest("Estoque insuficiente.");
                material.QuantidadeEstoque -= dto.Quantidade;
            }

            var movimento = new MovimentoEstoque
            {
                MaterialId = dto.MaterialId,
                Material = material, // instância do banco
                Quantidade = dto.Quantidade,
                ValorTotal = dto.Quantidade * material.CustoUnitario,
                Tipo = tipo,
                Data = DateTime.UtcNow,
                Observacao = dto.Observacao
            };

            _context.MovimentosEstoque.Add(movimento);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Movimento registrado",saldo = material.QuantidadeEstoque });
        }

        [HttpGet("movimentos")]
        public async Task<ActionResult> ListarMovimentos([FromQuery] int? materialId)
        {
            var query = _context.MovimentosEstoque
                .Include(m => m.Material)
                .AsNoTracking()
                .OrderByDescending(m => m.Data)
                .AsQueryable();

            if (materialId.HasValue) query = query.Where(m => m.MaterialId == materialId.Value);
            var lista = await query.ToListAsync();
            return Ok(lista);
        }
    }
}
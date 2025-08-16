using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoConstrucao.API.Data;
using ProjetoConstrucao.API.Models;

namespace ProjetoConstrucao.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MateriaisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MateriaisController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Material>>> GetAll()
            => Ok(await _context.Materiais.AsNoTracking().ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Material>> GetById(int id)
        {
            var mat = await _context.Materiais.FindAsync(id);
            return mat is null ? NotFound() : Ok(mat);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Material material)
        {
            if (string.IsNullOrWhiteSpace(material.Nome)) return BadRequest("Nome obrigatório.");
            if (string.IsNullOrWhiteSpace(material.UnidadeMedida)) return BadRequest("Unidade obrigatória.");
            if (material.CustoUnitario < 0) return BadRequest("Custo inválido.");

            _context.Materiais.Add(material);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = material.Id }, material);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, Material input)
        {
            var mat = await _context.Materiais.FindAsync(id);
            if (mat is null) return NotFound();

            mat.Nome = input.Nome;
            mat.UnidadeMedida = input.UnidadeMedida;
            mat.CustoUnitario = input.CustoUnitario;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var mat = await _context.Materiais.FindAsync(id);
            if (mat is null) return NotFound();

            _context.Materiais.Remove(mat);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using StockControl.Application.Services;
using StockControl.Models;

namespace StockControl.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstoqueController : ControllerBase
    {
        private readonly EstoqueService _estoqueService;
        public EstoqueController(EstoqueService estoqueService) => _estoqueService = estoqueService;

        [HttpPost("Movimentar")]
        public async Task<ActionResult> Movimentar([FromBody] MovimentoEstoqueCreateDTO dto)
        {
            var result = await _estoqueService.MovimentarAsync(dto);
            if(!result.sucesso) return BadRequest(result.mensagem);

            return Ok(new { mensagem = result.mensagem,saldo = result.saldo });
        }

        [HttpGet("Movimentos")]
        public async Task<ActionResult> ListarMovimentos([FromQuery] int? materialId)
        {
            var lista = await _estoqueService.ListarMovimentosAsync(materialId);
            return Ok(lista);
        }
    }
}
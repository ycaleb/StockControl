using Microsoft.AspNetCore.Mvc;
using StockControl.Application.Services;
using StockControl.Models;

namespace StockControl.Controllers;
[ApiController]
[Route("api/[controller]")]
public class MaterialController : ControllerBase
{
    private readonly MaterialService _materialService;
    public MaterialController(MaterialService materialService) => _materialService = materialService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Material>>> GetAll()
        => Ok(await _materialService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Material>> GetById(int id)
    {
        var mat = await _materialService.GetByIdAsync(id);
        return mat is null ? NotFound() : Ok(mat);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Material material)
    {
        var result = await _materialService.CreateAsync(material);
        if(!result.sucesso) return BadRequest(result.mensagem);
        return CreatedAtAction(nameof(GetById),new { id = result.material!.Id },result.material);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id,[FromBody] Material input)
    {
        var result = await _materialService.UpdateAsync(id,input);
        return result.sucesso ? NoContent() : NotFound(result.mensagem);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _materialService.DeleteAsync(id);
        return result.sucesso ? NoContent() : NotFound(result.mensagem);
    }
}
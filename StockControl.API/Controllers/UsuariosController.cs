using Microsoft.AspNetCore.Mvc;
using StockControl.Application.Services;
using StockControl.Models;

namespace StockControl.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly UsuarioService _usuarioService;
    public UsuariosController(UsuarioService usuarioService) => _usuarioService = usuarioService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetAll()
        => Ok(await _usuarioService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Usuario>> GetById(int id)
    {
        var user = await _usuarioService.GetByIdAsync(id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] UsuarioDto dto)
    {
        var usuario = new Usuario { Nome = dto.Nome,Cpf = dto.Cpf };
        var result = await _usuarioService.CreateAsync(usuario,dto.Senha);

        if(!result.sucesso) return BadRequest(result.mensagem);
        return CreatedAtAction(nameof(GetById),new { id = result.usuario!.Id },result.usuario);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id,[FromBody] UsuarioDto dto)
    {
        var input = new Usuario { Nome = dto.Nome,Cpf = dto.Cpf };
        var result = await _usuarioService.UpdateAsync(id,input,dto.Senha);
        return result.sucesso ? NoContent() : NotFound(result.mensagem);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _usuarioService.DeleteAsync(id);
        return result.sucesso ? NoContent() : NotFound(result.mensagem);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _usuarioService.LoginAsync(dto.Nome,dto.Senha);
        if(!result.sucesso) return Unauthorized(result.mensagem);
        return Ok(new { result.usuario!.Id,result.usuario.Nome });
    }

    public record LoginDto(string Nome,string Senha);

}

public record UsuarioDto(string Nome,string Senha,string? Cpf);

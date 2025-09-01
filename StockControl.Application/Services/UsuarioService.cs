using Microsoft.AspNetCore.Identity;
using StockControl.Application.Interfaces;
using StockControl.Models;

namespace StockControl.Application.Services;
public class UsuarioService
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly PasswordHasher<Usuario> _passwordHasher;

    public UsuarioService(IUsuarioRepository usuarioRepo)
    {
        _usuarioRepo = usuarioRepo;
        _passwordHasher = new PasswordHasher<Usuario>();
    }

    public async Task<List<Usuario>> GetAllAsync()
        => await _usuarioRepo.ListAllAsync();

    public async Task<Usuario?> GetByIdAsync(int id)
        => await _usuarioRepo.GetByIdAsync(id);

    public async Task<(bool sucesso, string mensagem, Usuario? usuario)> CreateAsync(Usuario usuario,string senha)
    {
        if(string.IsNullOrWhiteSpace(usuario.Nome))
            return (false, "Nome obrigatório", null);

        if(string.IsNullOrWhiteSpace(senha) || senha.Length < 8)
            return (false, "Senha deve ter pelo menos 8 caracteres", null);

        usuario.SenhaHash = _passwordHasher.HashPassword(usuario,senha);

        await _usuarioRepo.AddAsync(usuario);
        return (true, "Usuário criado com sucesso", usuario);
    }

    public async Task<(bool sucesso, string mensagem)> UpdateAsync(int id,Usuario input,string novaSenha)
    {
        var user = await _usuarioRepo.GetByIdAsync(id);
        if(user is null) return (false, "Usuário não encontrado");

        user.Nome = input.Nome;
        user.SenhaHash = _passwordHasher.HashPassword(user,novaSenha);

        await _usuarioRepo.UpdateAsync(user);
        return (true, "Usuário atualizado com sucesso");
    }

    public async Task<(bool sucesso, string mensagem, Usuario? usuario)> LoginAsync(string nome,string senha)
    {
        var users = await _usuarioRepo.ListAllAsync();
        var user = users.FirstOrDefault(u => u.Nome == nome);
        if(user is null) return (false, "Usuário não encontrado", null);

        var result = _passwordHasher.VerifyHashedPassword(user,user.SenhaHash,senha);
        if(result == PasswordVerificationResult.Failed)
            return (false, "Senha incorreta", null);

        return (true, "Login bem-sucedido", user);
    }

    public async Task<(bool sucesso, string mensagem)> DeleteAsync(int id)
    {
        var user = await _usuarioRepo.GetByIdAsync(id);
        if(user is null) return (false, "Usuário não encontrado");

        await _usuarioRepo.RemoveAsync(user);
        return (true, "Usuário removido com sucesso");
    }
}

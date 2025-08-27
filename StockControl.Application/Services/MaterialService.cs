
using StockControl.Application.Interfaces;
using StockControl.Models;

namespace StockControl.Application.Services;
public class MaterialService
{
    private readonly IMaterialRepository _materialRepo;

    public MaterialService(IMaterialRepository materialRepo)
    {
        _materialRepo = materialRepo;
    }

    public async Task<List<Material>> GetAllAsync()
        => await _materialRepo.ListAllAsync();

    public async Task<Material?> GetByIdAsync(int id)
        => await _materialRepo.GetByIdAsync(id);

    public async Task<(bool sucesso, string mensagem, Material? material)> CreateAsync(Material material)
    {
        if(string.IsNullOrWhiteSpace(material.Nome))
            return (false, "Nome obrigatório", null);
        if(string.IsNullOrWhiteSpace(material.UnidadeMedida))
            return (false, "Unidade obrigatória", null);
        if(material.CustoUnitario < 0)
            return (false, "Custo inválido", null);

        await _materialRepo.AddAsync(material);
        return (true, "Material criado com sucesso", material);
    }

    public async Task<(bool sucesso, string mensagem)> UpdateAsync(int id,Material input)
    {
        var mat = await _materialRepo.GetByIdAsync(id);
        if(mat is null) return (false, "Material não encontrado");

        mat.Nome = input.Nome;
        mat.UnidadeMedida = input.UnidadeMedida;
        mat.CustoUnitario = input.CustoUnitario;
        await _materialRepo.UpdateAsync(mat);

        return (true, "Atualizado com sucesso");
    }

    public async Task<(bool sucesso, string mensagem)> DeleteAsync(int id)
    {
        var mat = await _materialRepo.GetByIdAsync(id);
        if(mat is null) return (false, "Material não encontrado");

        await _materialRepo.RemoveAsync(mat);
        return (true, "Removido com sucesso");
    }
}

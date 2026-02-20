using Kimbito.Domain.Entities;
using Kimbito.Domain.Interfaces;
using Kimbito.Services.Dtos;
using Kimbito.Shared;

namespace Kimbito.Services;

public class CategoriaService
{
    private readonly ICategoria _repo;

    public CategoriaService(ICategoria repo)
    {
        _repo = repo;
    }

    public async Task<ApiResponse<CategoriaDto?>> ObterPorId(Guid id)
    {
        var c = await _repo.ObterPorId(id);
        if (c == null) return ApiResponse<CategoriaDto?>.Error(null, 404, "Categoria não encontrada");
        return ApiResponse<CategoriaDto?>.Success(MapToDto(c), 200);
    }

    public async Task<ApiResponse<List<CategoriaDto>>> ListarActivas()
    {
        var list = await _repo.ListarActivas();
        return ApiResponse<List<CategoriaDto>>.Success(list.Select(MapToDto).ToList(), 200);
    }

    public async Task<ApiResponse<List<CategoriaDto>>> ListarTodas()
    {
        var list = await _repo.ListarTodas();
        return ApiResponse<List<CategoriaDto>>.Success(list.Select(MapToDto).ToList(), 200);
    }

    public async Task<ApiResponse<CategoriaDto?>> Criar(CriarAtualizarCategoriaDto dto)
    {
        var slug = string.IsNullOrWhiteSpace(dto.Slug) ? GerarSlug(dto.Nome) : dto.Slug;
        var c = new Categoria
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome,
            Slug = slug,
            Descricao = dto.Descricao,
            Ordem = dto.Ordem,
            Activa = dto.Activa
        };
        var created = await _repo.Criar(c);
        return created == null
            ? ApiResponse<CategoriaDto?>.Error(null, 500, "Erro ao criar categoria")
            : ApiResponse<CategoriaDto?>.Success(MapToDto(created), 201);
    }

    public async Task<ApiResponse<CategoriaDto?>> Atualizar(Guid id, CriarAtualizarCategoriaDto dto)
    {
        var c = await _repo.ObterPorId(id);
        if (c == null) return ApiResponse<CategoriaDto?>.Error(null, 404, "Categoria não encontrada");
        c.Nome = dto.Nome;
        c.Slug = string.IsNullOrWhiteSpace(dto.Slug) ? GerarSlug(dto.Nome) : dto.Slug;
        c.Descricao = dto.Descricao;
        c.Ordem = dto.Ordem;
        c.Activa = dto.Activa;
        var updated = await _repo.Atualizar(c);
        return updated == null
            ? ApiResponse<CategoriaDto?>.Error(null, 500, "Erro ao atualizar")
            : ApiResponse<CategoriaDto?>.Success(MapToDto(updated), 200);
    }

    public async Task<ApiResponse<bool>> Eliminar(Guid id)
    {
        var ok = await _repo.Eliminar(id);
        return ApiResponse<bool>.Success(ok, ok ? 200 : 404, ok ? "Eliminada" : "Categoria não encontrada");
    }

    private static CategoriaDto MapToDto(Categoria c) => new()
    {
        Id = c.Id,
        Nome = c.Nome,
        Slug = c.Slug,
        Descricao = c.Descricao,
        Ordem = c.Ordem,
        Activa = c.Activa
    };

    private static string GerarSlug(string nome) =>
        string.Join("-", nome.Trim().ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries));
}

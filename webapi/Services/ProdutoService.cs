using Kimbito.Domain.Entities;
using Kimbito.Domain.Interfaces;
using Kimbito.Services.Dtos;
using Kimbito.Shared;

namespace Kimbito.Services;

public class ProdutoService
{
    private readonly IProduto _repo;

    public ProdutoService(IProduto repo)
    {
        _repo = repo;
    }

    public async Task<ApiResponse<ProdutoDto?>> ObterPorId(Guid id)
    {
        var p = await _repo.ObterPorId(id);
        if (p == null) return ApiResponse<ProdutoDto?>.Error(null, 404, "Produto não encontrado");
        return ApiResponse<ProdutoDto?>.Success(MapToDto(p), 200);
    }

    public async Task<ApiResponse<ProdutoDto?>> ObterPorSlug(string slug)
    {
        var p = await _repo.ObterPorSlug(slug);
        if (p == null) return ApiResponse<ProdutoDto?>.Error(null, 404, "Produto não encontrado");
        return ApiResponse<ProdutoDto?>.Success(MapToDto(p), 200);
    }

    public async Task<ApiResponse<List<ProdutoDto>>> ListarActivos(Guid? categoriaId = null, string? pesquisa = null)
    {
        var list = await _repo.ListarActivos(categoriaId, pesquisa);
        return ApiResponse<List<ProdutoDto>>.Success(list.Select(MapToDto).ToList(), 200);
    }

    public async Task<ApiResponse<List<ProdutoDto>>> ListarTodas(Guid? categoriaId = null, string? pesquisa = null)
    {
        var list = await _repo.ListarTodas(categoriaId, pesquisa);
        return ApiResponse<List<ProdutoDto>>.Success(list.Select(MapToDto).ToList(), 200);
    }

    public async Task<ApiResponse<ProdutoDto?>> Criar(CriarAtualizarProdutoDto dto)
    {
        var slug = string.IsNullOrWhiteSpace(dto.Slug) ? GerarSlug(dto.Nome) : dto.Slug;
        var p = new Produto
        {
            Id = Guid.NewGuid(),
            CategoriaId = dto.CategoriaId,
            Nome = dto.Nome,
            Slug = slug,
            Descricao = dto.Descricao,
            Preco = dto.Preco,
            Stock = dto.Stock,
            ImagemUrl = dto.ImagemUrl,
            Activo = dto.Activo
        };
        var created = await _repo.Criar(p);
        return created == null
            ? ApiResponse<ProdutoDto?>.Error(null, 500, "Erro ao criar produto")
            : ApiResponse<ProdutoDto?>.Success(MapToDto(created), 201);
    }

    public async Task<ApiResponse<ProdutoDto?>> Atualizar(Guid id, CriarAtualizarProdutoDto dto)
    {
        var p = await _repo.ObterPorId(id);
        if (p == null) return ApiResponse<ProdutoDto?>.Error(null, 404, "Produto não encontrado");
        p.CategoriaId = dto.CategoriaId;
        p.Nome = dto.Nome;
        p.Slug = string.IsNullOrWhiteSpace(dto.Slug) ? GerarSlug(dto.Nome) : dto.Slug;
        p.Descricao = dto.Descricao;
        p.Preco = dto.Preco;
        p.Stock = dto.Stock;
        p.ImagemUrl = dto.ImagemUrl;
        p.Activo = dto.Activo;
        var updated = await _repo.Atualizar(p);
        return updated == null
            ? ApiResponse<ProdutoDto?>.Error(null, 500, "Erro ao atualizar")
            : ApiResponse<ProdutoDto?>.Success(MapToDto(updated), 200);
    }

    public async Task<ApiResponse<bool>> ActualizarStock(Guid id, int quantidade)
    {
        var ok = await _repo.ActualizarStock(id, quantidade);
        return ApiResponse<bool>.Success(ok, ok ? 200 : 404);
    }

    public async Task<ApiResponse<bool>> Eliminar(Guid id)
    {
        var ok = await _repo.Eliminar(id);
        return ApiResponse<bool>.Success(ok, ok ? 200 : 404);
    }

    private static ProdutoDto MapToDto(Produto p) => new()
    {
        Id = p.Id,
        CategoriaId = p.CategoriaId,
        CategoriaNome = p.Categoria?.Nome,
        Nome = p.Nome,
        Slug = p.Slug,
        Descricao = p.Descricao,
        Preco = p.Preco,
        Stock = p.Stock,
        ImagemUrl = p.ImagemUrl,
        Activo = p.Activo
    };

    private static string GerarSlug(string nome) =>
        string.Join("-", nome.Trim().ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries));
}

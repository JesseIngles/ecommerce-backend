using Kimbito.Domain.Entities;
using Kimbito.Domain.Interfaces;
using Kimbito.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Kimbito.Infra.Repositories;

public class ProdutoRepository : IProduto
{
    private readonly KimbitoDbContext _db;

    public ProdutoRepository(KimbitoDbContext db)
    {
        _db = db;
    }

    public async Task<Produto?> ObterPorId(Guid id)
    {
        return await _db.Produtos.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Produto?> ObterPorSlug(string slug)
    {
        return await _db.Produtos.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<IEnumerable<Produto>> ListarActivos(Guid? categoriaId = null, string? pesquisa = null)
    {
        var q = _db.Produtos.Include(p => p.Categoria).Where(p => p.Activo);
        if (categoriaId.HasValue) q = q.Where(p => p.CategoriaId == categoriaId.Value);
        if (!string.IsNullOrWhiteSpace(pesquisa))
            q = q.Where(p => p.Nome.Contains(pesquisa) || (p.Descricao != null && p.Descricao.Contains(pesquisa)));
        return await q.OrderBy(p => p.Nome).ToListAsync();
    }

    public async Task<IEnumerable<Produto>> ListarTodas(Guid? categoriaId = null, string? pesquisa = null)
    {
        var q = _db.Produtos.Include(p => p.Categoria).AsQueryable();
        if (categoriaId.HasValue) q = q.Where(p => p.CategoriaId == categoriaId.Value);
        if (!string.IsNullOrWhiteSpace(pesquisa))
            q = q.Where(p => p.Nome.Contains(pesquisa) || (p.Descricao != null && p.Descricao.Contains(pesquisa)));
        return await q.OrderBy(p => p.Nome).ToListAsync();
    }

    public async Task<Produto?> Criar(Produto produto)
    {
        _db.Produtos.Add(produto);
        await _db.SaveChangesAsync();
        return produto;
    }

    public async Task<Produto?> Atualizar(Produto produto)
    {
        var existente = await _db.Produtos.FindAsync(produto.Id);
        if (existente == null) return null;
        existente.CategoriaId = produto.CategoriaId;
        existente.Nome = produto.Nome;
        existente.Slug = produto.Slug;
        existente.Descricao = produto.Descricao;
        existente.Preco = produto.Preco;
        existente.Stock = produto.Stock;
        existente.ImagemUrl = produto.ImagemUrl;
        existente.Activo = produto.Activo;
        existente.DataAtualizacao = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return existente;
    }

    public async Task<bool> ActualizarStock(Guid id, int quantidade)
    {
        var p = await _db.Produtos.FindAsync(id);
        if (p == null) return false;
        p.Stock = quantidade;
        p.DataAtualizacao = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Eliminar(Guid id)
    {
        var p = await _db.Produtos.FindAsync(id);
        if (p == null) return false;
        _db.Produtos.Remove(p);
        await _db.SaveChangesAsync();
        return true;
    }
}

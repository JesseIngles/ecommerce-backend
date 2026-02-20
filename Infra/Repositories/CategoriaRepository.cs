using Kimbito.Domain.Entities;
using Kimbito.Domain.Interfaces;
using Kimbito.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Kimbito.Infra.Repositories;

public class CategoriaRepository : ICategoria
{
    private readonly KimbitoDbContext _db;

    public CategoriaRepository(KimbitoDbContext db)
    {
        _db = db;
    }

    public async Task<Categoria?> ObterPorId(Guid id)
    {
        return await _db.Categorias.FindAsync(id);
    }

    public async Task<Categoria?> ObterPorSlug(string slug)
    {
        return await _db.Categorias.FirstOrDefaultAsync(c => c.Slug == slug);
    }

    public async Task<IEnumerable<Categoria>> ListarActivas()
    {
        return await _db.Categorias.Where(c => c.Activa).OrderBy(c => c.Ordem).ThenBy(c => c.Nome).ToListAsync();
    }

    public async Task<IEnumerable<Categoria>> ListarTodas()
    {
        return await _db.Categorias.OrderBy(c => c.Ordem).ThenBy(c => c.Nome).ToListAsync();
    }

    public async Task<Categoria?> Criar(Categoria categoria)
    {
        _db.Categorias.Add(categoria);
        await _db.SaveChangesAsync();
        return categoria;
    }

    public async Task<Categoria?> Atualizar(Categoria categoria)
    {
        var existente = await _db.Categorias.FindAsync(categoria.Id);
        if (existente == null) return null;
        existente.Nome = categoria.Nome;
        existente.Slug = categoria.Slug;
        existente.Descricao = categoria.Descricao;
        existente.Ordem = categoria.Ordem;
        existente.Activa = categoria.Activa;
        existente.DataAtualizacao = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return existente;
    }

    public async Task<bool> Eliminar(Guid id)
    {
        var c = await _db.Categorias.FindAsync(id);
        if (c == null) return false;
        _db.Categorias.Remove(c);
        await _db.SaveChangesAsync();
        return true;
    }
}

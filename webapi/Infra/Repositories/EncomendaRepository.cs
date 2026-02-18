using Kimbito.Domain.Entities;
using Kimbito.Domain.Enums;
using Kimbito.Domain.Interfaces;
using Kimbito.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Kimbito.Infra.Repositories;

public class EncomendaRepository : IEncomenda
{
    private readonly KimbitoDbContext _db;

    public EncomendaRepository(KimbitoDbContext db)
    {
        _db = db;
    }

    public async Task<Encomenda?> ObterPorId(Guid id)
    {
        return await _db.Encomendas
            .Include(e => e.Itens)
            .Include(e => e.Morada)
            .Include(e => e.FormaPagamento)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Encomenda>> ListarPorUtilizador(Guid utilizadorId)
    {
        return await _db.Encomendas
            .Include(e => e.Itens)
            .Include(e => e.FormaPagamento)
            .Where(e => e.UtilizadorId == utilizadorId)
            .OrderByDescending(e => e.DataEncomenda)
            .ToListAsync();
    }

    public async Task<IEnumerable<Encomenda>> ListarTodas(EstadoEncomenda? estado = null)
    {
        var q = _db.Encomendas.Include(e => e.Itens).Include(e => e.Utilizador).Include(e => e.FormaPagamento).AsQueryable();
        if (estado.HasValue) q = q.Where(e => e.Estado == estado.Value);
        return await q.OrderByDescending(e => e.DataEncomenda).ToListAsync();
    }

    public async Task<Encomenda?> Criar(Encomenda encomenda)
    {
        _db.Encomendas.Add(encomenda);
        foreach (var item in encomenda.Itens)
            _db.EncomendaItens.Add(item);
        await _db.SaveChangesAsync();
        return encomenda;
    }

    public async Task<Encomenda?> AtualizarEstado(Guid id, EstadoEncomenda estado)
    {
        var e = await _db.Encomendas.FindAsync(id);
        if (e == null) return null;
        e.Estado = estado;
        e.DataAtualizacaoEstado = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return e;
    }

    public async Task<bool> PertenceAoUtilizador(Guid encomendaId, Guid utilizadorId)
    {
        return await _db.Encomendas.AnyAsync(e => e.Id == encomendaId && e.UtilizadorId == utilizadorId);
    }
}

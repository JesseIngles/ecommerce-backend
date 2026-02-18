using Kimbito.Domain.Entities;
using Kimbito.Domain.Interfaces;
using Kimbito.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Kimbito.Infra.Repositories;

public class FormaPagamentoRepository : IFormaPagamento
{
    private readonly KimbitoDbContext _db;

    public FormaPagamentoRepository(KimbitoDbContext db)
    {
        _db = db;
    }

    public async Task<FormaPagamento?> ObterPorId(Guid id)
    {
        return await _db.FormasPagamento.FindAsync(id);
    }

    public async Task<IEnumerable<FormaPagamento>> ListarActivas()
    {
        return await _db.FormasPagamento.Where(f => f.Activa).OrderBy(f => f.Ordem).ThenBy(f => f.Nome).ToListAsync();
    }

    public async Task<IEnumerable<FormaPagamento>> ListarTodas()
    {
        return await _db.FormasPagamento.OrderBy(f => f.Ordem).ThenBy(f => f.Nome).ToListAsync();
    }

    public async Task<FormaPagamento?> Criar(FormaPagamento forma)
    {
        _db.FormasPagamento.Add(forma);
        await _db.SaveChangesAsync();
        return forma;
    }

    public async Task<FormaPagamento?> Atualizar(FormaPagamento forma)
    {
        var existente = await _db.FormasPagamento.FindAsync(forma.Id);
        if (existente == null) return null;
        existente.Nome = forma.Nome;
        existente.Descricao = forma.Descricao;
        existente.Tipo = forma.Tipo;
        existente.IbanOuDados = forma.IbanOuDados;
        existente.Activa = forma.Activa;
        existente.Ordem = forma.Ordem;
        await _db.SaveChangesAsync();
        return existente;
    }

    public async Task<bool> Eliminar(Guid id)
    {
        var f = await _db.FormasPagamento.FindAsync(id);
        if (f == null) return false;
        _db.FormasPagamento.Remove(f);
        await _db.SaveChangesAsync();
        return true;
    }
}

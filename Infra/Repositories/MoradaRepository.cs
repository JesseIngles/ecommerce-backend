using Kimbito.Domain.Entities;
using Kimbito.Domain.Interfaces;
using Kimbito.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Kimbito.Infra.Repositories;

public class MoradaRepository : IMorada
{
    private readonly KimbitoDbContext _db;

    public MoradaRepository(KimbitoDbContext db)
    {
        _db = db;
    }

    public async Task<Morada?> ObterPorId(Guid id)
    {
        return await _db.Moradas.FindAsync(id);
    }

    public async Task<IEnumerable<Morada>> ListarPorUtilizador(Guid utilizadorId)
    {
        return await _db.Moradas.Where(m => m.UtilizadorId == utilizadorId).OrderByDescending(m => m.Predefinida).ThenBy(m => m.Nome).ToListAsync();
    }

    public async Task<Morada?> Criar(Morada morada)
    {
        if (morada.Predefinida)
        {
            var outras = await _db.Moradas.Where(m => m.UtilizadorId == morada.UtilizadorId).ToListAsync();
            foreach (var o in outras) o.Predefinida = false;
            _db.Moradas.UpdateRange(outras);
        }
        _db.Moradas.Add(morada);
        await _db.SaveChangesAsync();
        return morada;
    }

    public async Task<Morada?> Atualizar(Morada morada)
    {
        var existente = await _db.Moradas.FindAsync(morada.Id);
        if (existente == null) return null;
        existente.Nome = morada.Nome;
        existente.Linha1 = morada.Linha1;
        existente.Linha2 = morada.Linha2;
        existente.CodigoPostal = morada.CodigoPostal;
        existente.Localidade = morada.Localidade;
        existente.Pais = morada.Pais;
        existente.Telefone = morada.Telefone;
        existente.Predefinida = morada.Predefinida;
        if (morada.Predefinida)
        {
            var outras = await _db.Moradas.Where(m => m.UtilizadorId == existente.UtilizadorId && m.Id != existente.Id).ToListAsync();
            foreach (var o in outras) o.Predefinida = false;
            _db.Moradas.UpdateRange(outras);
        }
        await _db.SaveChangesAsync();
        return existente;
    }

    public async Task<bool> Eliminar(Guid id)
    {
        var m = await _db.Moradas.FindAsync(id);
        if (m == null) return false;
        _db.Moradas.Remove(m);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> PertenceAoUtilizador(Guid moradaId, Guid utilizadorId)
    {
        return await _db.Moradas.AnyAsync(m => m.Id == moradaId && m.UtilizadorId == utilizadorId);
    }
}

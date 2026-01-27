using Kimbito.Domain.Entities;
using Kimbito.Domain.Interfaces;
using Kimbito.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Kimbito.Infra.Repositories;

public class UtilizadorRepository : IUtilizador
{
    private readonly KimbitoDbContext _db;
    public UtilizadorRepository(KimbitoDbContext dbContext)
    {
        _db = dbContext;   
    }

    public async Task<Utilizador?> AtualizarUtilizador(Guid? Id, Utilizador utilizador)
    {
        var utilizadorExistente = await _db.Utilizadores.FirstOrDefaultAsync(u => u.Id == Id);

        if(utilizadorExistente == null)
            return null;

        utilizadorExistente.NomeUsuario = utilizador.NomeUsuario;
        utilizadorExistente.Email = utilizador.Email;
        
        _db.Utilizadores.Update(utilizadorExistente);
        await _db.SaveChangesAsync();

        return utilizadorExistente;

    }

    public async Task<Utilizador?> ObterUtilizador(Guid? Id)
    {
        var utilizador = await _db.Utilizadores.FirstOrDefaultAsync(u => u.Id == Id);

        if(utilizador == null)
            return null;

        return utilizador;
    }
}
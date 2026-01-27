using System.Security.Claims;
using Kimbito.Domain.Entities;
using Kimbito.Domain.Interfaces;
using Kimbito.Infra.Database;
using Microsoft.EntityFrameworkCore;
using BCryptNet = DevOne.Security.Cryptography.BCrypt;

namespace Kimbito.Infra.Repositories;

public class AutenticacaoRepository : IAutenticacao
{
    private readonly KimbitoDbContext _db;
    public AutenticacaoRepository(KimbitoDbContext dbContext)
    {
        _db = dbContext;
    }

    public async Task<Utilizador?> CadastrarUtilizador(Utilizador utilizador)
    {
        if(utilizador == null)
            return null;

        _db.Utilizadores.Add(utilizador);

        await _db.SaveChangesAsync();

        return utilizador;
    }
    private string generateUtilizadorToken(Utilizador utilizador)
    {
        var claims = new Claim[]
        {
            
        };
        return string.Empty;
    }

    private bool check(string supossedPassword, string password)
    {
        return true;
    }

    public async Task<string?> Login(string usernameOrEmail, string passWord)
    {
        if(string.IsNullOrWhiteSpace(usernameOrEmail) || string.IsNullOrWhiteSpace(passWord))
            return null;

        var utilizador = await _db.Utilizadores.FirstOrDefaultAsync(u => 
            u.NomeUsuario == usernameOrEmail || u.Email == usernameOrEmail
        );

        bool gerarToken = (utilizador != null && BCryptNet.BCryptHelper.CheckPassword(passWord, utilizador.Senha));

        if(!gerarToken)
            return null;

        return generateUtilizadorToken(utilizador);
    }
}
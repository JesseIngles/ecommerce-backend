using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Kimbito.Domain.Entities;
using Kimbito.Domain.Interfaces;
using Kimbito.Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        string utilizadorToken = string.Empty;
        string _jwtSecret = "your_jwt_secret_here";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim("Id", utilizador.Id.ToString()),
            new Claim("Email", utilizador.Email), 
            new Claim("nome", utilizador.NomeUsuario),
        };

        var token = new JwtSecurityToken(
            issuer: "Ecommerce",
            audience: "Ecommerce",
            claims: claims,
            expires: DateTime.Now.AddMonths(1),
            signingCredentials: creds
        );

        utilizadorToken = new JwtSecurityTokenHandler().WriteToken(token);

        return utilizadorToken;
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
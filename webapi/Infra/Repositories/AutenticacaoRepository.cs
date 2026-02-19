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
    private readonly IConfiguration _config;

    public AutenticacaoRepository(KimbitoDbContext dbContext, IConfiguration config)
    {
        _db = dbContext;
        _config = config;
    }

    public async Task<Utilizador?> CadastrarUtilizador(Utilizador utilizador)
    {
        if (utilizador == null)
            return null;
        if (utilizador.Id == default)
            utilizador.Id = Guid.NewGuid();

        _db.Utilizadores.Add(utilizador);

        await _db.SaveChangesAsync();

        return utilizador;
    }
    private string generateUtilizadorToken(Utilizador utilizador)
    {
        var jwtSecret = _config["Jwt:Secret"] ?? "uH3q7W!9fP$k8VzR2mB@xE4sT#yL1QwD";
        var issuer = _config["Jwt:Issuer"] ?? "KimbitoEcommerce";
        var audience = _config["Jwt:Audience"] ?? "KimbitoEcommerce";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim("Id", utilizador.Id.ToString()),
            new Claim("Email", utilizador.Email),
            new Claim(System.Security.Claims.ClaimTypes.Role, utilizador.Nivel.ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMonths(1),
            signingCredentials: creds
        );

        var utilizadorToken = new JwtSecurityTokenHandler().WriteToken(token);
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

        if (!gerarToken || utilizador == null)
            return null;

        return generateUtilizadorToken(utilizador);
    }
}
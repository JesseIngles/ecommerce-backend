using Kimbito.Services.Dtos;

namespace Kimbito.Domain.Entities;

public class Utilizador
{
    public Guid Id {get;set;}
    public string? Avatar {get;set;}
    public string NomeUsuario {get;set;}
    public string Email {get;set;}
    public string Senha {get;set;}
    public string? GoogleId {get;set;}
    public DateTime DataRegistro {get;set;} = DateTime.UtcNow;
    public DateTime DataAtualizacao {get;set;} = DateTime.UtcNow;
    public DateTime DataExclusao {get;set;}
    public Utilizador(string nomeUsuario, string email, string senha)
    {
        NomeUsuario = nomeUsuario;
        Email = email;
        Senha = senha;
    }
}
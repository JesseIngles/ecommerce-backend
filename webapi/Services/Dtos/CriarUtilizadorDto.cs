namespace Kimbito.Services.Dtos;

public class CriarUtilizadorDto
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string? Avatar {get;set;}
    public string Senha {get;set;}

    public CriarUtilizadorDto(string nome, string email, string? avatar, string senha)
    {
        Nome = nome;
        Email = email;
        Avatar = avatar;
        Senha = senha;
    }
}
namespace Kimbito.Services.Dtos;

public class UtilizadorDto
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string? Avatar {get;set;}

    public UtilizadorDto(string nome, string email, string? avatar)
    {
        Nome = nome;
        Email = email;
        Avatar = avatar;
    }
}
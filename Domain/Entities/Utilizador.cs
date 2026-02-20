using Kimbito.Domain.Enums;

namespace Kimbito.Domain.Entities;

public class Utilizador
{
    public Guid Id { get; set; }
    public string? Avatar { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string? GoogleId { get; set; }
    public NivelUtilizador Nivel { get; set; } = NivelUtilizador.Cliente;
    public DateTime DataRegistro { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataExclusao { get; set; }

    public Utilizador(string nomeUsuario, string email, string senha, NivelUtilizador nivel = NivelUtilizador.Cliente)
    {
        NomeUsuario = nomeUsuario;
        Email = email;
        Senha = senha;
        Nivel = nivel;
    }
}
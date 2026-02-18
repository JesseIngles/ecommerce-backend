using Kimbito.Domain.Enums;

namespace Kimbito.Services.Dtos;

public class CriarUtilizadorDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string Senha { get; set; } = string.Empty;
    /// <summary>Nível de acesso. No registo público só Cliente é permitido.</summary>
    public NivelUtilizador? Nivel { get; set; }
}
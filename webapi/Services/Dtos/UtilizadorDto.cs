using Kimbito.Domain.Enums;

namespace Kimbito.Services.Dtos;

public class UtilizadorDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public NivelUtilizador Nivel { get; set; }
}
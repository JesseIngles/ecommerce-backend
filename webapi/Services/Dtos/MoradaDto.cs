namespace Kimbito.Services.Dtos;

public class MoradaDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Linha1 { get; set; } = string.Empty;
    public string? Linha2 { get; set; }
    public string CodigoPostal { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public bool Predefinida { get; set; }
}

public class CriarAtualizarMoradaDto
{
    public string Nome { get; set; } = string.Empty;
    public string Linha1 { get; set; } = string.Empty;
    public string? Linha2 { get; set; }
    public string CodigoPostal { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string Pais { get; set; } = "Portugal";
    public string? Telefone { get; set; }
    public bool Predefinida { get; set; }
}

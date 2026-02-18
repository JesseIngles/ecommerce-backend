namespace Kimbito.Domain.Entities;

public class Morada
{
    public Guid Id { get; set; }
    public Guid UtilizadorId { get; set; }
    public Utilizador? Utilizador { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Linha1 { get; set; } = string.Empty;
    public string? Linha2 { get; set; }
    public string CodigoPostal { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string Pais { get; set; } = "Portugal";
    public string? Telefone { get; set; }
    public bool Predefinida { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}

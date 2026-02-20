namespace Kimbito.Services.Dtos;

public class CategoriaDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public int Ordem { get; set; }
    public bool Activa { get; set; }
}

public class CriarAtualizarCategoriaDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string? Descricao { get; set; }
    public int Ordem { get; set; }
    public bool Activa { get; set; } = true;
}

namespace Kimbito.Services.Dtos;

public class ProdutoDto
{
    public Guid Id { get; set; }
    public Guid CategoriaId { get; set; }
    public string? CategoriaNome { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Stock { get; set; }
    public string? ImagemUrl { get; set; }
    public bool Activo { get; set; }
}

public class CriarAtualizarProdutoDto
{
    public Guid CategoriaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Stock { get; set; }
    public string? ImagemUrl { get; set; }
    public bool Activo { get; set; } = true;
}

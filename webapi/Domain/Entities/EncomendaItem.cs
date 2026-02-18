namespace Kimbito.Domain.Entities;

public class EncomendaItem
{
    public Guid Id { get; set; }
    public Guid EncomendaId { get; set; }
    public Encomenda? Encomenda { get; set; }
    public Guid ProdutoId { get; set; }
    public Produto? Produto { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public decimal PrecoUnitario { get; set; }
    public int Quantidade { get; set; }
}

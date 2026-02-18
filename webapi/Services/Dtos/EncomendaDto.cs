using Kimbito.Domain.Enums;

namespace Kimbito.Services.Dtos;

public class EncomendaItemDto
{
    public Guid ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public decimal PrecoUnitario { get; set; }
    public int Quantidade { get; set; }
}

public class CriarEncomendaDto
{
    public Guid MoradaId { get; set; }
    public Guid? FormaPagamentoId { get; set; }
    public string? Notas { get; set; }
    public List<EncomendaItemDto> Itens { get; set; } = new();
}

public class EncomendaItemRespostaDto
{
    public Guid Id { get; set; }
    public Guid ProdutoId { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public decimal PrecoUnitario { get; set; }
    public int Quantidade { get; set; }
}

public class EncomendaDto
{
    public Guid Id { get; set; }
    public Guid UtilizadorId { get; set; }
    public Guid MoradaId { get; set; }
    public Guid? FormaPagamentoId { get; set; }
    public string? FormaPagamentoNome { get; set; }
    public EstadoEncomenda Estado { get; set; }
    public decimal Total { get; set; }
    public string? Notas { get; set; }
    public DateTime DataEncomenda { get; set; }
    public DateTime? DataAtualizacaoEstado { get; set; }
    public List<EncomendaItemRespostaDto> Itens { get; set; } = new();
    public MoradaDto? Morada { get; set; }
}

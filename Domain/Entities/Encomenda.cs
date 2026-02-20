using Kimbito.Domain.Enums;

namespace Kimbito.Domain.Entities;

public class Encomenda
{
    public Guid Id { get; set; }
    public Guid UtilizadorId { get; set; }
    public Utilizador? Utilizador { get; set; }
    public Guid MoradaId { get; set; }
    public Morada? Morada { get; set; }
    public Guid? FormaPagamentoId { get; set; }
    public FormaPagamento? FormaPagamento { get; set; }
    public EstadoEncomenda Estado { get; set; } = EstadoEncomenda.Pendente;
    public decimal Total { get; set; }
    public string? Notas { get; set; }
    public DateTime DataEncomenda { get; set; } = DateTime.UtcNow;
    public DateTime? DataAtualizacaoEstado { get; set; }

    public ICollection<EncomendaItem> Itens { get; set; } = new List<EncomendaItem>();
}

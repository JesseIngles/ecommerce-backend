namespace Kimbito.Domain.Entities;

public class FormaPagamento
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    /// <summary>Ex: MBWay, Transferência, Multibanco, Cartão</summary>
    public string Tipo { get; set; } = string.Empty;
    public string? IbanOuDados { get; set; }
    public bool Activa { get; set; } = true;
    public int Ordem { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}

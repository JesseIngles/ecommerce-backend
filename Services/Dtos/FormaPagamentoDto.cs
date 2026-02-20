namespace Kimbito.Services.Dtos;

public class FormaPagamentoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string? IbanOuDados { get; set; }
    public bool Activa { get; set; }
    public int Ordem { get; set; }
}

public class CriarAtualizarFormaPagamentoDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string? IbanOuDados { get; set; }
    public bool Activa { get; set; } = true;
    public int Ordem { get; set; }
}

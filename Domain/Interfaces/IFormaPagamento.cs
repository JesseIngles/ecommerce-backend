using Kimbito.Domain.Entities;

namespace Kimbito.Domain.Interfaces;

public interface IFormaPagamento
{
    Task<FormaPagamento?> ObterPorId(Guid id);
    Task<IEnumerable<FormaPagamento>> ListarActivas();
    Task<IEnumerable<FormaPagamento>> ListarTodas();
    Task<FormaPagamento?> Criar(FormaPagamento forma);
    Task<FormaPagamento?> Atualizar(FormaPagamento forma);
    Task<bool> Eliminar(Guid id);
}

using Kimbito.Domain.Entities;
using Kimbito.Domain.Enums;

namespace Kimbito.Domain.Interfaces;

public interface IEncomenda
{
    Task<Encomenda?> ObterPorId(Guid id);
    Task<IEnumerable<Encomenda>> ListarPorUtilizador(Guid utilizadorId);
    Task<IEnumerable<Encomenda>> ListarTodas(EstadoEncomenda? estado = null);
    Task<Encomenda?> Criar(Encomenda encomenda);
    Task<Encomenda?> AtualizarEstado(Guid id, EstadoEncomenda estado);
    Task<bool> PertenceAoUtilizador(Guid encomendaId, Guid utilizadorId);
}

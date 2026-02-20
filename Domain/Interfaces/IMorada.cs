using Kimbito.Domain.Entities;

namespace Kimbito.Domain.Interfaces;

public interface IMorada
{
    Task<Morada?> ObterPorId(Guid id);
    Task<IEnumerable<Morada>> ListarPorUtilizador(Guid utilizadorId);
    Task<Morada?> Criar(Morada morada);
    Task<Morada?> Atualizar(Morada morada);
    Task<bool> Eliminar(Guid id);
    Task<bool> PertenceAoUtilizador(Guid moradaId, Guid utilizadorId);
}

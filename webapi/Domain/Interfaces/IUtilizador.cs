using Kimbito.Domain.Entities;

namespace Kimbito.Domain.Interfaces;

public interface IUtilizador
{
    Task<Utilizador?> ObterUtilizador(Guid? Id);
    Task<Utilizador?> AtualizarUtilizador(Guid? Id, Utilizador utilizador);
}
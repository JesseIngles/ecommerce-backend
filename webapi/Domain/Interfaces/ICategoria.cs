using Kimbito.Domain.Entities;

namespace Kimbito.Domain.Interfaces;

public interface ICategoria
{
    Task<Categoria?> ObterPorId(Guid id);
    Task<Categoria?> ObterPorSlug(string slug);
    Task<IEnumerable<Categoria>> ListarActivas();
    Task<IEnumerable<Categoria>> ListarTodas();
    Task<Categoria?> Criar(Categoria categoria);
    Task<Categoria?> Atualizar(Categoria categoria);
    Task<bool> Eliminar(Guid id);
}

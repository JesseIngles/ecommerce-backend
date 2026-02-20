using Kimbito.Domain.Entities;

namespace Kimbito.Domain.Interfaces;

public interface IProduto
{
    Task<Produto?> ObterPorId(Guid id);
    Task<Produto?> ObterPorSlug(string slug);
    Task<IEnumerable<Produto>> ListarActivos(Guid? categoriaId = null, string? pesquisa = null);
    Task<IEnumerable<Produto>> ListarTodas(Guid? categoriaId = null, string? pesquisa = null);
    Task<Produto?> Criar(Produto produto);
    Task<Produto?> Atualizar(Produto produto);
    Task<bool> ActualizarStock(Guid id, int quantidade);
    Task<bool> Eliminar(Guid id);
}

using Kimbito.Domain.Entities;
using Kimbito.Domain.Interfaces;
using Kimbito.Services.Dtos;
using Kimbito.Shared;

namespace Kimbito.Services;

public class FormaPagamentoService
{
    private readonly IFormaPagamento _repo;

    public FormaPagamentoService(IFormaPagamento repo)
    {
        _repo = repo;
    }

    public async Task<ApiResponse<FormaPagamentoDto?>> ObterPorId(Guid id)
    {
        var f = await _repo.ObterPorId(id);
        if (f == null) return ApiResponse<FormaPagamentoDto?>.Error(null, 404, "Forma de pagamento não encontrada");
        return ApiResponse<FormaPagamentoDto?>.Success(MapToDto(f), 200);
    }

    public async Task<ApiResponse<List<FormaPagamentoDto>>> ListarActivas()
    {
        var list = await _repo.ListarActivas();
        return ApiResponse<List<FormaPagamentoDto>>.Success(list.Select(MapToDto).ToList(), 200);
    }

    public async Task<ApiResponse<List<FormaPagamentoDto>>> ListarTodas()
    {
        var list = await _repo.ListarTodas();
        return ApiResponse<List<FormaPagamentoDto>>.Success(list.Select(MapToDto).ToList(), 200);
    }

    public async Task<ApiResponse<FormaPagamentoDto?>> Criar(CriarAtualizarFormaPagamentoDto dto)
    {
        var f = new FormaPagamento
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Tipo = dto.Tipo,
            IbanOuDados = dto.IbanOuDados,
            Activa = dto.Activa,
            Ordem = dto.Ordem
        };
        var created = await _repo.Criar(f);
        return created == null
            ? ApiResponse<FormaPagamentoDto?>.Error(null, 500, "Erro ao criar")
            : ApiResponse<FormaPagamentoDto?>.Success(MapToDto(created), 201);
    }

    public async Task<ApiResponse<FormaPagamentoDto?>> Atualizar(Guid id, CriarAtualizarFormaPagamentoDto dto)
    {
        var f = await _repo.ObterPorId(id);
        if (f == null) return ApiResponse<FormaPagamentoDto?>.Error(null, 404, "Forma de pagamento não encontrada");
        f.Nome = dto.Nome;
        f.Descricao = dto.Descricao;
        f.Tipo = dto.Tipo;
        f.IbanOuDados = dto.IbanOuDados;
        f.Activa = dto.Activa;
        f.Ordem = dto.Ordem;
        var updated = await _repo.Atualizar(f);
        return updated == null
            ? ApiResponse<FormaPagamentoDto?>.Error(null, 500, "Erro ao atualizar")
            : ApiResponse<FormaPagamentoDto?>.Success(MapToDto(updated), 200);
    }

    public async Task<ApiResponse<bool>> Eliminar(Guid id)
    {
        var ok = await _repo.Eliminar(id);
        return ApiResponse<bool>.Success(ok, ok ? 200 : 404);
    }

    private static FormaPagamentoDto MapToDto(FormaPagamento f) => new()
    {
        Id = f.Id,
        Nome = f.Nome,
        Descricao = f.Descricao,
        Tipo = f.Tipo,
        IbanOuDados = f.IbanOuDados,
        Activa = f.Activa,
        Ordem = f.Ordem
    };
}

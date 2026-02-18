using Kimbito.Domain.Entities;
using Kimbito.Domain.Interfaces;
using Kimbito.Services.Dtos;
using Kimbito.Shared;

namespace Kimbito.Services;

public class MoradaService
{
    private readonly IMorada _repo;

    public MoradaService(IMorada repo)
    {
        _repo = repo;
    }

    public async Task<ApiResponse<MoradaDto?>> ObterPorId(Guid id, Guid utilizadorId)
    {
        var m = await _repo.ObterPorId(id);
        if (m == null) return ApiResponse<MoradaDto?>.Error(null, 404, "Morada não encontrada");
        if (m.UtilizadorId != utilizadorId) return ApiResponse<MoradaDto?>.Error(null, 403, "Acesso negado");
        return ApiResponse<MoradaDto?>.Success(MapToDto(m), 200);
    }

    public async Task<ApiResponse<List<MoradaDto>>> ListarPorUtilizador(Guid utilizadorId)
    {
        var list = await _repo.ListarPorUtilizador(utilizadorId);
        return ApiResponse<List<MoradaDto>>.Success(list.Select(MapToDto).ToList(), 200);
    }

    public async Task<ApiResponse<MoradaDto?>> Criar(CriarAtualizarMoradaDto dto, Guid utilizadorId)
    {
        var m = new Morada
        {
            Id = Guid.NewGuid(),
            UtilizadorId = utilizadorId,
            Nome = dto.Nome,
            Linha1 = dto.Linha1,
            Linha2 = dto.Linha2,
            CodigoPostal = dto.CodigoPostal,
            Localidade = dto.Localidade,
            Pais = dto.Pais,
            Telefone = dto.Telefone,
            Predefinida = dto.Predefinida
        };
        var created = await _repo.Criar(m);
        return created == null
            ? ApiResponse<MoradaDto?>.Error(null, 500, "Erro ao criar morada")
            : ApiResponse<MoradaDto?>.Success(MapToDto(created), 201);
    }

    public async Task<ApiResponse<MoradaDto?>> Atualizar(Guid id, CriarAtualizarMoradaDto dto, Guid utilizadorId)
    {
        if (!await _repo.PertenceAoUtilizador(id, utilizadorId))
            return ApiResponse<MoradaDto?>.Error(null, 403, "Acesso negado");
        var m = await _repo.ObterPorId(id);
        if (m == null) return ApiResponse<MoradaDto?>.Error(null, 404, "Morada não encontrada");
        m.Nome = dto.Nome;
        m.Linha1 = dto.Linha1;
        m.Linha2 = dto.Linha2;
        m.CodigoPostal = dto.CodigoPostal;
        m.Localidade = dto.Localidade;
        m.Pais = dto.Pais;
        m.Telefone = dto.Telefone;
        m.Predefinida = dto.Predefinida;
        var updated = await _repo.Atualizar(m);
        return ApiResponse<MoradaDto?>.Success(MapToDto(updated!), 200);
    }

    public async Task<ApiResponse<bool>> Eliminar(Guid id, Guid utilizadorId)
    {
        if (!await _repo.PertenceAoUtilizador(id, utilizadorId))
            return ApiResponse<bool>.Error(false, 403, "Acesso negado");
        var ok = await _repo.Eliminar(id);
        return ApiResponse<bool>.Success(ok, 200);
    }

    private static MoradaDto MapToDto(Morada m) => new()
    {
        Id = m.Id,
        Nome = m.Nome,
        Linha1 = m.Linha1,
        Linha2 = m.Linha2,
        CodigoPostal = m.CodigoPostal,
        Localidade = m.Localidade,
        Pais = m.Pais,
        Telefone = m.Telefone,
        Predefinida = m.Predefinida
    };
}

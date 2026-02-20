using Kimbito.Domain.Entities;
using Kimbito.Domain.Interfaces;
using Kimbito.Services.Dtos;
using Kimbito.Shared;

namespace Kimbito.Services;

public class UtilizadorService
{
    private readonly IUtilizador _utilizadorRepository;

    public UtilizadorService(IUtilizador utilizadorRepository)
    {
        _utilizadorRepository = utilizadorRepository;
    }

    public async Task<ApiResponse<UtilizadorDto?>> ObterUtilizador(Guid? Id)
    {
        
        var utilizador = await _utilizadorRepository.ObterUtilizador(Id);

        if(utilizador == null)
            return ApiResponse<UtilizadorDto?>.Error(null, 404, "Utilizador nao encontrado");
        
        return ApiResponse<UtilizadorDto?>.Success(
            new UtilizadorDto { Nome = utilizador.NomeUsuario, Email = utilizador.Email, Avatar = utilizador.Avatar, Nivel = utilizador.Nivel },
            200,
            "Utilizador obtido com sucesso"
        );
    }
   

    public async Task<ApiResponse<UtilizadorDto?>> AtualizarUtilizador(Guid Id, UtilizadorDto dto)
    {
        var utilizador = await _utilizadorRepository.ObterUtilizador(Id);

         if(utilizador == null)
            return ApiResponse<UtilizadorDto?>.Error(null, 404, "Utilizador nao encontrado");

        bool dadosValidos = (!string.IsNullOrEmpty(utilizador.NomeUsuario) 
            && !string.IsNullOrEmpty(utilizador.Email)
            && !string.IsNullOrEmpty(utilizador.Senha));

        if (dadosValidos)
        {
            var atualizado = new Utilizador(dto.Nome, dto.Email, utilizador.Senha, dto.Nivel) { Avatar = dto.Avatar };
            utilizador = await _utilizadorRepository.AtualizarUtilizador(Id, atualizado);
            return ApiResponse<UtilizadorDto?>.Success(dto, 200, "Sucesso ao atualizar utilizador");
        }

        return ApiResponse<UtilizadorDto?>.Error(dto, 400, "Dados Inválidos");
    }
}
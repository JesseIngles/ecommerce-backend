using Kimbito.Domain.Entities;
using Kimbito.Domain.Interfaces;
using Kimbito.Services.Dtos;
using Kimbito.Shared;
using BCryptNet = DevOne.Security.Cryptography.BCrypt;

namespace Kimbito.Services;

public class AutenticacaoService
{
    private readonly IAutenticacao _autenticacaoRepository;

    public AutenticacaoService(IAutenticacao autenticacaoRepository)
    {
        _autenticacaoRepository = autenticacaoRepository;
    }

    public async Task<ApiResponse<Utilizador>> CadastrarUtilizador(CriarUtilizadorDto utilizador)
    {
        var novoUtilizador = new Utilizador(utilizador.Nome, utilizador.Email, BCryptNet.BCryptHelper.HashPassword(utilizador.Senha, BCryptNet.BCryptHelper.GenerateSalt()));

        var result = await _autenticacaoRepository.CadastrarUtilizador(novoUtilizador);

        if(result == null)
            return ApiResponse<Utilizador>.Error(null, 500, "Erro ao cadastrar utilizador");
        
        return ApiResponse<Utilizador>.Success(result, 201);
    }
    public async Task<ApiResponse<string>> Login(string usernameOrEmail, string passWord)
    {
        var token = await _autenticacaoRepository.Login(usernameOrEmail, passWord);

        if(string.IsNullOrWhiteSpace(token))
            return ApiResponse<string>.Error(null, 401, "Credenciais inválidas");

        return ApiResponse<string>.Success(token, 200, "Login realizado com sucesso");
    }
}
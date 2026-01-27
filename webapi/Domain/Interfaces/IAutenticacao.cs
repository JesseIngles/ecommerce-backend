using Kimbito.Domain.Entities;
using Kimbito.Shared;

namespace Kimbito.Domain.Interfaces;

public interface IAutenticacao
{
  Task<Utilizador?> CadastrarUtilizador(Utilizador utilizador);
  Task<string?> Login(string usernameOrEmail, string passWord);
}
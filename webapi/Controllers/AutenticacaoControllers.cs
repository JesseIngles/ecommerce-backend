using Kimbito.Services;
using Kimbito.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.EventLog;

namespace Kimbito.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AutenticacaoController : Controller
{
    private ILogger<AutenticacaoController> _logger;
    private AutenticacaoService _autenticacaoService;
    public AutenticacaoController(ILogger<AutenticacaoController> logger, AutenticacaoService autenticacaoService)
    {
        _logger = logger;
        _autenticacaoService = autenticacaoService;
    }

    [HttpPost("Cadastrar")]
    public async Task<IActionResult> Cadastrar([FromBody] CriarUtilizadorDto utilizadorDto)
    {
        var result = await _autenticacaoService.CadastrarUtilizador(utilizadorDto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("Google/Cadastrar")]
    public async Task<IActionResult> GoogleCadastrar()
    {
        return Ok();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] string usernameOrEmail = "user@example.com", string passWord = "")
    {
        var result = await _autenticacaoService.Login(usernameOrEmail, passWord);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("Google/Login")]
    public async Task<IActionResult> GoogleLogin([FromBody] string googleToken)
    {
        return Ok();
    }

    [HttpPost("Recuperar/Solicitacao")]
    public async Task<IActionResult> Recuperar([FromBody] string email)
    {
        return Ok();
    }

    [Authorize]
    [HttpPost("Recuperar/NovaSenha")]
    public async Task<IActionResult> TrocarSenha([FromBody] int otp)
    {
        return Ok();
    }



    
}


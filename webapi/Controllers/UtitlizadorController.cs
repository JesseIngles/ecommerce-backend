using System.Security.Claims;
using Kimbito.Services;
using Kimbito.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.EventLog;

namespace Kimbito.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UtilizadorController : Controller
{
    private ILogger<UtilizadorController> _logger;
    private UtilizadorService _utilizadorService;
    public UtilizadorController(ILogger<UtilizadorController> logger, UtilizadorService utilizadorService)
    {
        _logger = logger;
        _utilizadorService = utilizadorService;
    }

    [Authorize]
    [HttpPatch]
    public async Task<IActionResult> Atualizar([FromForm] UtilizadorDto utilizador, [FromQuery] int utilizadorId)
    {
        
        return Ok();
    }

    [Authorize]
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> ObterPorId([FromQuery] Guid? Id)
    {
        Guid? userId = Guid.Parse(User.FindFirstValue("Id")!);
        if(userId is null && Id is null)
            return BadRequest();
        else 
            userId = Id;

        var result = await _utilizadorService.ObterUtilizador(userId);

        return StatusCode(result.StatusCode, result);
    }


   





     
    
}


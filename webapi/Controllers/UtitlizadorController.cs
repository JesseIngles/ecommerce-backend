using System.Security.Claims;
using Kimbito.Services;
using Kimbito.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kimbito.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UtilizadorController : ControllerBase
{
    private readonly UtilizadorService _utilizadorService;

    public UtilizadorController(UtilizadorService utilizadorService)
    {
        _utilizadorService = utilizadorService;
    }

    private Guid? UtilizadorId => Guid.TryParse(User.FindFirstValue("Id"), out var id) ? id : null;

    [HttpPatch]
    public async Task<IActionResult> Atualizar([FromBody] UtilizadorDto dto)
    {
        if (UtilizadorId == null) return Unauthorized();
        var result = await _utilizadorService.AtualizarUtilizador(UtilizadorId.Value, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        if (UtilizadorId == null) return Unauthorized();
        var result = await _utilizadorService.ObterUtilizador(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("me")]
    public async Task<IActionResult> ObterMeuPerfil()
    {
        if (UtilizadorId == null) return Unauthorized();
        var result = await _utilizadorService.ObterUtilizador(UtilizadorId);
        return StatusCode(result.StatusCode, result);
    }
}


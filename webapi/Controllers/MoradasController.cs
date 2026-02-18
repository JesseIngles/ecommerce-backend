using System.Security.Claims;
using Kimbito.Services;
using Kimbito.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kimbito.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MoradasController : ControllerBase
{
    private readonly MoradaService _service;

    public MoradasController(MoradaService service)
    {
        _service = service;
    }

    private Guid? UtilizadorId => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("Id"), out var id) ? id : null;

    [HttpGet]
    public async Task<IActionResult> ListarMinhas()
    {
        if (UtilizadorId == null) return Unauthorized();
        var result = await _service.ListarPorUtilizador(UtilizadorId.Value);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        if (UtilizadorId == null) return Unauthorized();
        var result = await _service.ObterPorId(id, UtilizadorId.Value);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarAtualizarMoradaDto dto)
    {
        if (UtilizadorId == null) return Unauthorized();
        var result = await _service.Criar(dto, UtilizadorId.Value);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] CriarAtualizarMoradaDto dto)
    {
        if (UtilizadorId == null) return Unauthorized();
        var result = await _service.Atualizar(id, dto, UtilizadorId.Value);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Eliminar(Guid id)
    {
        if (UtilizadorId == null) return Unauthorized();
        var result = await _service.Eliminar(id, UtilizadorId.Value);
        return StatusCode(result.StatusCode, result);
    }
}

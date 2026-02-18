using System.Security.Claims;
using Kimbito.Services;
using Kimbito.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kimbito.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EncomendasController : ControllerBase
{
    private readonly EncomendaService _service;

    public EncomendasController(EncomendaService service)
    {
        _service = service;
    }

    private Guid? UtilizadorId => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("Id"), out var id) ? id : null;
    private bool IsAdminOuGestor => User.IsInRole("Admin") || User.IsInRole("Gestor");

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
        var result = await _service.ObterPorId(id, UtilizadorId, IsAdminOuGestor);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarEncomendaDto dto)
    {
        if (UtilizadorId == null) return Unauthorized();
        var result = await _service.Criar(dto, UtilizadorId.Value);
        return StatusCode(result.StatusCode, result);
    }
}

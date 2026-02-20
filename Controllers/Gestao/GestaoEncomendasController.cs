using Kimbito.Domain.Enums;
using Kimbito.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kimbito.Controllers.Gestao;

[ApiController]
[Route("api/gestao/[controller]")]
[Authorize(Roles = "Admin,Gestor")]
public class GestaoEncomendasController : ControllerBase
{
    private readonly EncomendaService _service;

    public GestaoEncomendasController(EncomendaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodas([FromQuery] EstadoEncomenda? estado)
    {
        var result = await _service.ListarTodas(estado);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var result = await _service.ObterPorId(id, null, true);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("{id:guid}/estado")]
    public async Task<IActionResult> AtualizarEstado(Guid id, [FromBody] EstadoEncomenda estado)
    {
        var result = await _service.AtualizarEstado(id, estado);
        return StatusCode(result.StatusCode, result);
    }
}

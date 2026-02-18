using Kimbito.Services;
using Kimbito.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kimbito.Controllers.Gestao;

[ApiController]
[Route("api/gestao/[controller]")]
[Authorize(Roles = "Admin,Gestor")]
public class GestaoCategoriasController : ControllerBase
{
    private readonly CategoriaService _service;

    public GestaoCategoriasController(CategoriaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodas()
    {
        var result = await _service.ListarTodas();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var result = await _service.ObterPorId(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarAtualizarCategoriaDto dto)
    {
        var result = await _service.Criar(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] CriarAtualizarCategoriaDto dto)
    {
        var result = await _service.Atualizar(id, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Eliminar(Guid id)
    {
        var result = await _service.Eliminar(id);
        return StatusCode(result.StatusCode, result);
    }
}

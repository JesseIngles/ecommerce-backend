using Kimbito.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kimbito.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly ProdutoService _service;

    public ProdutosController(ProdutoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] Guid? categoriaId, [FromQuery] string? pesquisa)
    {
        var result = await _service.ListarActivos(categoriaId, pesquisa);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var result = await _service.ObterPorId(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> ObterPorSlug(string slug)
    {
        var result = await _service.ObterPorSlug(slug);
        return StatusCode(result.StatusCode, result);
    }
}

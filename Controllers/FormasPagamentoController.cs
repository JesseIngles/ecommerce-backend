using Kimbito.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kimbito.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FormasPagamentoController : ControllerBase
{
    private readonly FormaPagamentoService _service;

    public FormasPagamentoController(FormaPagamentoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> ListarActivas()
    {
        var result = await _service.ListarActivas();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var result = await _service.ObterPorId(id);
        return StatusCode(result.StatusCode, result);
    }
}

using Kimbito.Domain.Entities;
using Kimbito.Domain.Enums;
using Kimbito.Domain.Interfaces;
using Kimbito.Services.Dtos;
using Kimbito.Shared;

namespace Kimbito.Services;

public class EncomendaService
{
    private readonly IEncomenda _encomendaRepo;
    private readonly IProduto _produtoRepo;
    private readonly IMorada _moradaRepo;

    public EncomendaService(IEncomenda encomendaRepo, IProduto produtoRepo, IMorada moradaRepo)
    {
        _encomendaRepo = encomendaRepo;
        _produtoRepo = produtoRepo;
        _moradaRepo = moradaRepo;
    }

    public async Task<ApiResponse<EncomendaDto?>> ObterPorId(Guid id, Guid? utilizadorId, bool isAdminOuGestor)
    {
        var e = await _encomendaRepo.ObterPorId(id);
        if (e == null) return ApiResponse<EncomendaDto?>.Error(null, 404, "Encomenda não encontrada");
        if (!isAdminOuGestor && (e.UtilizadorId != utilizadorId))
            return ApiResponse<EncomendaDto?>.Error(null, 403, "Acesso negado");
        return ApiResponse<EncomendaDto?>.Success(MapToDto(e), 200);
    }

    public async Task<ApiResponse<List<EncomendaDto>>> ListarPorUtilizador(Guid utilizadorId)
    {
        var list = await _encomendaRepo.ListarPorUtilizador(utilizadorId);
        return ApiResponse<List<EncomendaDto>>.Success(list.Select(MapToDto).ToList(), 200);
    }

    public async Task<ApiResponse<List<EncomendaDto>>> ListarTodas(EstadoEncomenda? estado = null)
    {
        var list = await _encomendaRepo.ListarTodas(estado);
        return ApiResponse<List<EncomendaDto>>.Success(list.Select(MapToDto).ToList(), 200);
    }

    public async Task<ApiResponse<EncomendaDto?>> Criar(CriarEncomendaDto dto, Guid utilizadorId)
    {
        if (dto.Itens == null || dto.Itens.Count == 0)
            return ApiResponse<EncomendaDto?>.Error(null, 400, "Encomenda sem itens");
        if (!await _moradaRepo.PertenceAoUtilizador(dto.MoradaId, utilizadorId))
            return ApiResponse<EncomendaDto?>.Error(null, 400, "Morada inválida");

        decimal total = 0;
        var itens = new List<EncomendaItem>();
        foreach (var item in dto.Itens)
        {
            var produto = await _produtoRepo.ObterPorId(item.ProdutoId);
            if (produto == null)
                return ApiResponse<EncomendaDto?>.Error(null, 400, $"Produto não encontrado: {item.ProdutoId}");
            if (produto.Stock < item.Quantidade)
                return ApiResponse<EncomendaDto?>.Error(null, 400, $"Stock insuficiente para {produto.Nome}");
            var preco = produto.Preco;
            total += preco * item.Quantidade;
            itens.Add(new EncomendaItem
            {
                Id = Guid.NewGuid(),
                ProdutoId = produto.Id,
                NomeProduto = produto.Nome,
                PrecoUnitario = preco,
                Quantidade = item.Quantidade
            });
        }

        var encomenda = new Encomenda
        {
            Id = Guid.NewGuid(),
            UtilizadorId = utilizadorId,
            MoradaId = dto.MoradaId,
            FormaPagamentoId = dto.FormaPagamentoId,
            Estado = EstadoEncomenda.Pendente,
            Total = total,
            Notas = dto.Notas
        };
        foreach (var item in itens)
        {
            item.EncomendaId = encomenda.Id;
            encomenda.Itens.Add(item);
        }

        var created = await _encomendaRepo.Criar(encomenda);
        if (created == null) return ApiResponse<EncomendaDto?>.Error(null, 500, "Erro ao criar encomenda");

        foreach (var item in created.Itens)
        {
            var prod = await _produtoRepo.ObterPorId(item.ProdutoId);
            if (prod != null)
                await _produtoRepo.ActualizarStock(item.ProdutoId, prod.Stock - item.Quantidade);
        }

        return ApiResponse<EncomendaDto?>.Success(MapToDto(created), 201);
    }

    public async Task<ApiResponse<EncomendaDto?>> AtualizarEstado(Guid id, EstadoEncomenda estado)
    {
        var e = await _encomendaRepo.AtualizarEstado(id, estado);
        if (e == null) return ApiResponse<EncomendaDto?>.Error(null, 404, "Encomenda não encontrada");
        var full = await _encomendaRepo.ObterPorId(id);
        return ApiResponse<EncomendaDto?>.Success(full != null ? MapToDto(full) : null, 200);
    }

    private static EncomendaDto MapToDto(Encomenda e) => new()
    {
        Id = e.Id,
        UtilizadorId = e.UtilizadorId,
        MoradaId = e.MoradaId,
        FormaPagamentoId = e.FormaPagamentoId,
        FormaPagamentoNome = e.FormaPagamento?.Nome,
        Estado = e.Estado,
        Total = e.Total,
        Notas = e.Notas,
        DataEncomenda = e.DataEncomenda,
        DataAtualizacaoEstado = e.DataAtualizacaoEstado,
        Itens = e.Itens.Select(i => new EncomendaItemRespostaDto
        {
            Id = i.Id,
            ProdutoId = i.ProdutoId,
            NomeProduto = i.NomeProduto,
            PrecoUnitario = i.PrecoUnitario,
            Quantidade = i.Quantidade
        }).ToList(),
        Morada = e.Morada != null ? new MoradaDto
        {
            Id = e.Morada.Id,
            Nome = e.Morada.Nome,
            Linha1 = e.Morada.Linha1,
            Linha2 = e.Morada.Linha2,
            CodigoPostal = e.Morada.CodigoPostal,
            Localidade = e.Morada.Localidade,
            Pais = e.Morada.Pais,
            Telefone = e.Morada.Telefone,
            Predefinida = e.Morada.Predefinida
        } : null
    };
}

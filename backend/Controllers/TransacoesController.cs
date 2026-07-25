using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GastosResidenciais.Api.Controllers;

/// <summary>
/// Endpoints para o cadastro de transações: criação e listagem.
/// (Edição e deleção não são exigidas pelo desafio.)
/// </summary>
[ApiController]
[Route("api/transacoes")]
public class TransacoesController : ControllerBase
{
    private readonly TransacaoService _transacaoService;

    public TransacoesController(TransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    /// <summary>Lista todas as transações cadastradas.</summary>
    [HttpGet]
    public ActionResult<List<TransacaoDto>> Listar()
    {
        return Ok(_transacaoService.ListarTodas());
    }

    /// <summary>
    /// Cadastra uma nova transação. Valida que a pessoa informada existe e que,
    /// se ela for menor de idade, apenas despesas estão sendo cadastradas.
    /// </summary>
    [HttpPost]
    public ActionResult<TransacaoDto> Criar([FromBody] CriarTransacaoDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var transacaoCriada = _transacaoService.Criar(dto);
            return CreatedAtAction(nameof(Listar), new { id = transacaoCriada.Id }, transacaoCriada);
        }
        catch (RegraDeNegocioException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}
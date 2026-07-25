using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GastosResidenciais.Api.Controllers;

/// <summary>
/// Endpoint de consulta de totais: lista todas as pessoas com seus totais de
/// receitas, despesas e saldo, além do total geral consolidado.
/// </summary>
[ApiController]
[Route("api/totais")]
public class TotaisController : ControllerBase
{
    private readonly TransacaoService _transacaoService;

    public TotaisController(TransacaoService transacaoService)
    {
        _transacaoService = transacaoService;
    }

    [HttpGet]
    public ActionResult<ConsultaTotaisDto> Consultar()
    {
        return Ok(_transacaoService.ConsultarTotais());
    }
}
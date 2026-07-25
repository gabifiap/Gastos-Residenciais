using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GastosResidenciais.Api.Controllers;

/// <summary>
/// Endpoints para o cadastro de pessoas: criação, listagem e remoção.
/// </summary>
[ApiController]
[Route("api/pessoas")]
public class PessoasController : ControllerBase
{
    private readonly PessoaService _pessoaService;

    public PessoasController(PessoaService pessoaService)
    {
        _pessoaService = pessoaService;
    }

    /// <summary>Lista todas as pessoas cadastradas.</summary>
    [HttpGet]
    public ActionResult<List<PessoaDto>> Listar()
    {
        return Ok(_pessoaService.ListarTodas());
    }

    /// <summary>Cadastra uma nova pessoa.</summary>
    [HttpPost]
    public ActionResult<PessoaDto> Criar([FromBody] CriarPessoaDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var pessoaCriada = _pessoaService.Criar(dto);
        return CreatedAtAction(nameof(Listar), new { id = pessoaCriada.Id }, pessoaCriada);
    }

    /// <summary>
    /// Remove uma pessoa cadastrada. Todas as transações associadas a ela
    /// também são removidas automaticamente.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public IActionResult Remover(Guid id)
    {
        var removida = _pessoaService.Remover(id);
        if (!removida)
        {
            return NotFound(new { mensagem = $"Nenhuma pessoa encontrada com o identificador '{id}'." });
        }

        return NoContent();
    }
}
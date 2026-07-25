using System.ComponentModel.DataAnnotations;
using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.DTOs;

/// <summary>
/// Dados necessários para cadastrar uma nova transação.
/// </summary>
public class CriarTransacaoDto
{
    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [MinLength(1, ErrorMessage = "A descrição não pode ser vazia.")]
    public string Descricao { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "O tipo da transação é obrigatório.")]
    public TipoTransacao Tipo { get; set; }

    [Required(ErrorMessage = "O identificador da pessoa é obrigatório.")]
    public Guid PessoaId { get; set; }
}

/// <summary>
/// Representação de uma transação retornada pela API.
/// </summary>
public class TransacaoDto
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public TipoTransacao Tipo { get; set; }
    public Guid PessoaId { get; set; }
}
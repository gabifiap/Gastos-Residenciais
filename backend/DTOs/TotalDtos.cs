namespace GastosResidenciais.Api.DTOs;

/// <summary>
/// Resumo financeiro (totais) de uma única pessoa.
/// </summary>
public class TotalPessoaDto
{
    public Guid PessoaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal Saldo { get; set; }
}

/// <summary>
/// Resposta consolidada da consulta de totais: o detalhamento por pessoa
/// e o total geral somando todas as pessoas cadastradas.
/// </summary>
public class ConsultaTotaisDto
{
    public List<TotalPessoaDto> Pessoas { get; set; } = new();
    public decimal TotalGeralReceitas { get; set; }
    public decimal TotalGeralDespesas { get; set; }
    public decimal SaldoGeral { get; set; }
}
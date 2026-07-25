namespace GastosResidenciais.Api.Models;

/// <summary>
/// Representa o tipo de uma transação financeira: uma entrada de dinheiro (Receita)
/// ou uma saída de dinheiro (Despesa).
/// </summary>
public enum TipoTransacao
{
    Despesa = 0,
    Receita = 1
}
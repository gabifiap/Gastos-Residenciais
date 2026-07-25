namespace GastosResidenciais.Api.Models;

/// <summary>
/// Representa uma pessoa cadastrada no sistema, à qual transações financeiras
/// podem ser associadas.
/// </summary>
public class Pessoa
{
    /// <summary>
    /// Identificador único da pessoa. Gerado automaticamente pelo sistema no momento do cadastro.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nome completo da pessoa.
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Idade da pessoa. Usada para aplicar a regra de negócio que restringe
    /// pessoas menores de 18 anos a registrarem apenas despesas.
    /// </summary>
    public int Idade { get; set; }

    /// <summary>
    /// Indica se a pessoa é menor de idade (menor de 18 anos), segundo a regra
    /// de negócio definida no desafio.
    /// </summary>
    public bool EhMenorDeIdade => Idade < 18;
}
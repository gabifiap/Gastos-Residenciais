namespace GastosResidenciais.Api.Models;

/// <summary>
/// Representa uma transação financeira (receita ou despesa) associada a uma pessoa.
/// </summary>
public class Transacao
{
    /// <summary>
    /// Identificador único da transação. Gerado automaticamente pelo sistema no momento do cadastro.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Descrição livre da transação (ex.: "Salário", "Mercado", "Mesada").
    /// </summary>
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Valor monetário da transação. Sempre armazenado como um valor positivo;
    /// o sinal (soma ou subtração) é definido pelo campo <see cref="Tipo"/>.
    /// </summary>
    public decimal Valor { get; set; }

    /// <summary>
    /// Indica se a transação é uma Receita ou uma Despesa.
    /// </summary>
    public TipoTransacao Tipo { get; set; }

    /// <summary>
    /// Identificador da pessoa à qual esta transação pertence. Deve corresponder
    /// a uma pessoa já existente no cadastro de pessoas.
    /// </summary>
    public Guid PessoaId { get; set; }
}
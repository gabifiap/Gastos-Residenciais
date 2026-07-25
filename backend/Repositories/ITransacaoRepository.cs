using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Repositories;

public interface ITransacaoRepository
{
    IReadOnlyList<Transacao> ListarTodas();
    IReadOnlyList<Transacao> ListarPorPessoa(Guid pessoaId);
    Transacao Adicionar(Transacao transacao);
    /// <summary>Remove todas as transações de uma pessoa (usado no cascade delete).</summary>
    void RemoverPorPessoa(Guid pessoaId);
}
using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Repositories;

/// <summary>
/// Repositório de transações, persistido em um arquivo JSON (Data/transacoes.json).
/// </summary>
public class TransacaoRepository : ITransacaoRepository
{
    private readonly JsonFileStore<Transacao> _store = new("transacoes.json");
    private readonly List<Transacao> _transacoes;
    private readonly object _travaMemoria = new();

    public TransacaoRepository()
    {
        _transacoes = _store.CarregarTudo();
    }

    public IReadOnlyList<Transacao> ListarTodas()
    {
        lock (_travaMemoria)
        {
            return _transacoes.ToList();
        }
    }

    public IReadOnlyList<Transacao> ListarPorPessoa(Guid pessoaId)
    {
        lock (_travaMemoria)
        {
            return _transacoes.Where(t => t.PessoaId == pessoaId).ToList();
        }
    }

    public Transacao Adicionar(Transacao transacao)
    {
        lock (_travaMemoria)
        {
            transacao.Id = Guid.NewGuid();
            _transacoes.Add(transacao);
            _store.SalvarTudo(_transacoes);
            return transacao;
        }
    }

    public void RemoverPorPessoa(Guid pessoaId)
    {
        lock (_travaMemoria)
        {
            var quantidadeRemovida = _transacoes.RemoveAll(t => t.PessoaId == pessoaId);
            if (quantidadeRemovida > 0)
            {
                _store.SalvarTudo(_transacoes);
            }
        }
    }
}
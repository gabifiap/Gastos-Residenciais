using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Repositories;

/// <summary>
/// Repositório de pessoas, persistido em um arquivo JSON (Data/pessoas.json).
/// Registrado como singleton para manter uma única cópia em memória durante
/// a execução da aplicação, sincronizada com o arquivo em disco.
/// </summary>
public class PessoaRepository : IPessoaRepository
{
    private readonly JsonFileStore<Pessoa> _store = new("pessoas.json");
    private readonly List<Pessoa> _pessoas;
    private readonly object _travaMemoria = new();

    public PessoaRepository()
    {
        _pessoas = _store.CarregarTudo();
    }

    public IReadOnlyList<Pessoa> ListarTodas()
    {
        lock (_travaMemoria)
        {
            // Retorna uma cópia para evitar que quem consome altere a lista interna.
            return _pessoas.ToList();
        }
    }

    public Pessoa? BuscarPorId(Guid id)
    {
        lock (_travaMemoria)
        {
            return _pessoas.FirstOrDefault(p => p.Id == id);
        }
    }

    public Pessoa Adicionar(Pessoa pessoa)
    {
        lock (_travaMemoria)
        {
            pessoa.Id = Guid.NewGuid();
            _pessoas.Add(pessoa);
            _store.SalvarTudo(_pessoas);
            return pessoa;
        }
    }

    public bool Remover(Guid id)
    {
        lock (_travaMemoria)
        {
            var pessoa = _pessoas.FirstOrDefault(p => p.Id == id);
            if (pessoa is null)
            {
                return false;
            }

            _pessoas.Remove(pessoa);
            _store.SalvarTudo(_pessoas);
            return true;
        }
    }
}
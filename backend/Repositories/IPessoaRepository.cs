using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Repositories;

public interface IPessoaRepository
{
    IReadOnlyList<Pessoa> ListarTodas();
    Pessoa? BuscarPorId(Guid id);
    Pessoa Adicionar(Pessoa pessoa);
    /// <summary>Remove a pessoa. Retorna false se ela não existir.</summary>
    bool Remover(Guid id);
}
using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Models;
using GastosResidenciais.Api.Repositories;

namespace GastosResidenciais.Api.Services;

/// <summary>
/// Contém as regras de negócio relacionadas ao cadastro de pessoas.
/// </summary>
public class PessoaService
{
    private readonly IPessoaRepository _pessoaRepository;
    private readonly ITransacaoRepository _transacaoRepository;

    public PessoaService(IPessoaRepository pessoaRepository, ITransacaoRepository transacaoRepository)
    {
        _pessoaRepository = pessoaRepository;
        _transacaoRepository = transacaoRepository;
    }

    public List<PessoaDto> ListarTodas()
    {
        return _pessoaRepository
            .ListarTodas()
            .Select(MapearParaDto)
            .ToList();
    }

    public PessoaDto Criar(CriarPessoaDto dto)
    {
        var pessoa = new Pessoa
        {
            Nome = dto.Nome.Trim(),
            Idade = dto.Idade
        };

        var pessoaCriada = _pessoaRepository.Adicionar(pessoa);
        return MapearParaDto(pessoaCriada);
    }

    /// <summary>
    /// Remove uma pessoa e, em cascata, todas as transações associadas a ela
    /// (conforme especificado no desafio).
    /// </summary>
    /// <returns>false se a pessoa não existir.</returns>
    public bool Remover(Guid id)
    {
        var pessoa = _pessoaRepository.BuscarPorId(id);
        if (pessoa is null)
        {
            return false;
        }

        _transacaoRepository.RemoverPorPessoa(id);
        _pessoaRepository.Remover(id);
        return true;
    }

    private static PessoaDto MapearParaDto(Pessoa pessoa)
    {
        return new PessoaDto
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Idade = pessoa.Idade,
            EhMenorDeIdade = pessoa.EhMenorDeIdade
        };
    }
}
using GastosResidenciais.Api.DTOs;
using GastosResidenciais.Api.Models;
using GastosResidenciais.Api.Repositories;

namespace GastosResidenciais.Api.Services;

/// <summary>
/// Contém as regras de negócio relacionadas ao cadastro de transações e à
/// consulta de totais.
/// </summary>
public class TransacaoService
{
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly IPessoaRepository _pessoaRepository;

    public TransacaoService(ITransacaoRepository transacaoRepository, IPessoaRepository pessoaRepository)
    {
        _transacaoRepository = transacaoRepository;
        _pessoaRepository = pessoaRepository;
    }

    public List<TransacaoDto> ListarTodas()
    {
        return _transacaoRepository
            .ListarTodas()
            .Select(MapearParaDto)
            .ToList();
    }

    /// <summary>
    /// Cadastra uma nova transação, validando:
    /// 1. Que a pessoa informada existe no cadastro de pessoas;
    /// 2. Que, caso a pessoa seja menor de idade (menor de 18 anos), apenas
    ///    despesas podem ser cadastradas para ela (nunca receitas).
    /// </summary>
    /// <exception cref="RegraDeNegocioException">
    /// Lançada quando a pessoa não existe ou quando a regra de menor de idade é violada.
    /// </exception>
    public TransacaoDto Criar(CriarTransacaoDto dto)
    {
        var pessoa = _pessoaRepository.BuscarPorId(dto.PessoaId);
        if (pessoa is null)
        {
            throw new RegraDeNegocioException(
                $"Não foi encontrada nenhuma pessoa cadastrada com o identificador '{dto.PessoaId}'.");
        }

        if (pessoa.EhMenorDeIdade && dto.Tipo == TipoTransacao.Receita)
        {
            throw new RegraDeNegocioException(
                $"'{pessoa.Nome}' é menor de idade ({pessoa.Idade} anos). " +
                "Para pessoas menores de idade, apenas despesas podem ser cadastradas.");
        }

        var transacao = new Transacao
        {
            Descricao = dto.Descricao.Trim(),
            Valor = dto.Valor,
            Tipo = dto.Tipo,
            PessoaId = dto.PessoaId
        };

        var transacaoCriada = _transacaoRepository.Adicionar(transacao);
        return MapearParaDto(transacaoCriada);
    }

    /// <summary>
    /// Calcula, para cada pessoa cadastrada, o total de receitas, o total de
    /// despesas e o saldo (receitas - despesas), além do total geral somando
    /// todas as pessoas.
    /// </summary>
    public ConsultaTotaisDto ConsultarTotais()
    {
        var pessoas = _pessoaRepository.ListarTodas();
        var transacoes = _transacaoRepository.ListarTodas();

        var totaisPorPessoa = new List<TotalPessoaDto>();

        foreach (var pessoa in pessoas)
        {
            var transacoesDaPessoa = transacoes.Where(t => t.PessoaId == pessoa.Id);

            var totalReceitas = transacoesDaPessoa
                .Where(t => t.Tipo == TipoTransacao.Receita)
                .Sum(t => t.Valor);

            var totalDespesas = transacoesDaPessoa
                .Where(t => t.Tipo == TipoTransacao.Despesa)
                .Sum(t => t.Valor);

            totaisPorPessoa.Add(new TotalPessoaDto
            {
                PessoaId = pessoa.Id,
                Nome = pessoa.Nome,
                TotalReceitas = totalReceitas,
                TotalDespesas = totalDespesas,
                Saldo = totalReceitas - totalDespesas
            });
        }

        return new ConsultaTotaisDto
        {
            Pessoas = totaisPorPessoa,
            TotalGeralReceitas = totaisPorPessoa.Sum(p => p.TotalReceitas),
            TotalGeralDespesas = totaisPorPessoa.Sum(p => p.TotalDespesas),
            SaldoGeral = totaisPorPessoa.Sum(p => p.Saldo)
        };
    }

    private static TransacaoDto MapearParaDto(Transacao transacao)
    {
        return new TransacaoDto
        {
            Id = transacao.Id,
            Descricao = transacao.Descricao,
            Valor = transacao.Valor,
            Tipo = transacao.Tipo,
            PessoaId = transacao.PessoaId
        };
    }
}
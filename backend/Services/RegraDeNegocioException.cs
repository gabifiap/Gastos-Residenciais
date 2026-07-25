namespace GastosResidenciais.Api.Services;

/// <summary>
/// Exceção lançada quando uma requisição viola uma regra de negócio do sistema
/// (ex.: tentar cadastrar receita para um menor de idade, ou referenciar uma
/// pessoa que não existe). É tratada nos controllers e traduzida em uma
/// resposta HTTP 400 (Bad Request) com uma mensagem amigável.
/// </summary>
public class RegraDeNegocioException : Exception
{
    public RegraDeNegocioException(string mensagem) : base(mensagem)
    {
    }
}
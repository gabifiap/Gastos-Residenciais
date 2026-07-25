using System.Text.Json;

namespace GastosResidenciais.Api.Repositories;

/// <summary>
/// Utilitário simples de persistência baseado em arquivo JSON.
/// Carrega uma lista de itens em memória na inicialização e regrava o arquivo
/// inteiro a cada alteração, garantindo que os dados sobrevivam ao fechamento da aplicação.
///
/// Optamos por essa abordagem (em vez de um banco de dados como SQLite via
/// Entity Framework) para manter o projeto livre de dependências externas via
/// NuGet, usando apenas a biblioteca padrão do .NET. Para um cenário de produção
/// real, a recomendação seria migrar para um banco relacional.
/// </summary>
public class JsonFileStore<T>
{
    private readonly string _caminhoArquivo;
    private readonly object _travaArquivo = new();
    private readonly JsonSerializerOptions _opcoesJson = new()
    {
        WriteIndented = true
    };

    public JsonFileStore(string nomeArquivo)
    {
        var diretorioDados = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(diretorioDados);
        _caminhoArquivo = Path.Combine(diretorioDados, nomeArquivo);
    }

    public List<T> CarregarTudo()
    {
        lock (_travaArquivo)
        {
            if (!File.Exists(_caminhoArquivo))
            {
                return new List<T>();
            }

            var conteudo = File.ReadAllText(_caminhoArquivo);
            if (string.IsNullOrWhiteSpace(conteudo))
            {
                return new List<T>();
            }

            return JsonSerializer.Deserialize<List<T>>(conteudo, _opcoesJson) ?? new List<T>();
        }
    }

    public void SalvarTudo(List<T> itens)
    {
        lock (_travaArquivo)
        {
            var json = JsonSerializer.Serialize(itens, _opcoesJson);
            File.WriteAllText(_caminhoArquivo, json);
        }
    }
}
using System.Text.Json.Serialization;
using GastosResidenciais.Api.Repositories;
using GastosResidenciais.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Repositórios registrados como singleton: cada um mantém uma lista em memória
// sincronizada com um arquivo JSON em disco (ver Repositories/JsonFileStore.cs),
// garantindo que os dados persistam entre execuções da aplicação.
builder.Services.AddSingleton<IPessoaRepository, PessoaRepository>();
builder.Services.AddSingleton<ITransacaoRepository, TransacaoRepository>();

// Serviços com as regras de negócio, com tempo de vida por requisição.
builder.Services.AddScoped<PessoaService>();
builder.Services.AddScoped<TransacaoService>();

// Serializa enums (ex.: Tipo da transação) como texto ("Despesa"/"Receita")
// em vez de número, para deixar a API mais legível para o front-end.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Libera acesso do front-end (React, rodando em outra porta) à API.
const string PoliticaCors = "PermitirFrontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(PoliticaCors, policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors(PoliticaCors);
app.UseAuthorization();
app.MapControllers();

app.Run();
# Estágio 1: compila a aplicação usando a imagem do SDK do .NET
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia só o arquivo de projeto primeiro, para aproveitar cache de camadas
COPY GastosResidenciais.Api.csproj ./
RUN dotnet restore

# Copia o restante do código e publica a versão de produção
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Estágio 2: imagem final, menor, só com o necessário para rodar
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "GastosResidenciais.Api.dll"]
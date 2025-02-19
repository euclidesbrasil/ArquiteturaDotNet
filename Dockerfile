FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
#
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
#
# Copia todos os arquivos para o contêiner
COPY /src .
RUN ls -la
#
# Restaura as dependências
WORKDIR /src/ArquiteturaDesafio.General.Api
RUN dotnet restore "ArquiteturaDesafio.General.Api.csproj"
#
WORKDIR "/src/ArquiteturaDesafio.General.Api"
RUN dotnet publish "ArquiteturaDesafio.General.Api.csproj" --configuration Release -o /app/publish --no-restore
#
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ArquiteturaDesafio.General.Api.dll"]
# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copiar os arquivos .csproj dos projetos para restaurar dependências
COPY Domain/Domain.csproj Domain/
COPY Api/Api.csproj Api/

# Restaurar as dependências no projeto principal (Api)
RUN dotnet restore Api/Api.csproj

# Copiar todo o código-fonte para /src
COPY . .

# Publicar o projeto Api
RUN dotnet publish Api/Api.csproj -c Release -o /app/out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Copiar os arquivos publicados da etapa de build
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "Api.dll"]

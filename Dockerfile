# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copiar o arquivo .csproj para /app
COPY Api/Api.csproj ./

# Restaurar dependências
RUN dotnet restore Api.csproj

# Copiar todo o código da pasta Api
COPY Api/. ./

# Publicar o projeto
RUN dotnet publish Api.csproj -c Release -o /app/out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Copiar arquivos publicados da etapa de build
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "Api.dll"]

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copiar o arquivo .csproj para /app
COPY BarberGo/BarberGo.csproj ./

# Restaurar dependências
RUN dotnet restore BarberGo.csproj

# Copiar todo o código da pasta BarberGo
COPY BarberGo/. ./

# Publicar o projeto
RUN dotnet publish BarberGo.csproj -c Release -o /app/out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Copiar arquivos publicados da etapa de build
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "BarberGo.dll"]

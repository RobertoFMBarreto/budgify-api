
# Ir buscar a imagem do .net 6.0 e guardar com image base
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
# vamos trabalhar no /app
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -o out

# *** guardar com build-env
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
# vamos trabalhar no build
WORKDIR /build

COPY --from=base /app/out .
ENTRYPOINT [ "dotnet", "wallets.dll" ]






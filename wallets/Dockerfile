
# Ir buscar a imagem do .net 6.0 e guardar com image base
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
# vamos trabalhar no /app
WORKDIR /app

EXPOSE 5001
EXPOSE 80

COPY . ./
RUN dotnet restore

RUN dotnet publish -c Release -o out

# *** guardar com build-env
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
# vamos trabalhar no build
WORKDIR /build

COPY --from=base /app/out .
ENTRYPOINT [ "dotnet", "wallets.dll" ]
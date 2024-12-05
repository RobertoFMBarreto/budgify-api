#Imagem a ser utilizada como base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base 
#definição do diretório de trabalho
WORKDIR /app
#Exposição da porta
EXPOSE 5072

#todos os ficheiros do projeto são movidos para o diretório /app
COPY . ./
#realizado o restauro da solução
RUN dotnet restore
#realizado o publish da solução
RUN dotnet publish -c Release -o out

#utilizada a mesma imagem utilizada acima para realizar o build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
#definido o diretório de trabalho como /build
WORKDIR /build
#são copiados todos os cheiros gerados no processo anterior para o diretório de build
COPY --from=base /app/out .
#definição do entrypoint para execução da solução que aponta para o ficheiro .dll da solução.
ENTRYPOINT ["dotnet", "BudgifyAPI.Auth.dll"]
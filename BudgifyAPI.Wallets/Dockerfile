FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app

EXPOSE 5006

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /build
COPY --from=base /app/out .
ENTRYPOINT ["dotnet", "accounts.dll"]
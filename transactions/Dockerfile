FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app

EXPOSE 5001

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS build-env 
WORKDIR /build
COPY --from=base /app/out .
ENTRYPOINT ["dotnet", "transaction.dll"]
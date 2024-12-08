FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base 
WORKDIR /app
EXPOSE 65082
EXPOSE 65083

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /build
COPY --from=base /app/out .
RUN dotnet dev-certs https --clean
RUN dotnet dev-certs https --trust


COPY /certs/shared.crt /usr/local/share/ca-certificates/shared.crt
RUN update-ca-certificates

ENTRYPOINT ["dotnet", "BudgifyAPI.Auth.CA.dll"]
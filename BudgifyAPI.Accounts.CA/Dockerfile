FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BudgifyAPI.Accounts.CA/BudgifyAPI.Accounts.CA.csproj", "BudgifyAPI.Accounts.CA/"]
RUN dotnet restore "BudgifyAPI.Accounts.CA/BudgifyAPI.Accounts.CA.csproj"
COPY . .
WORKDIR "/src/BudgifyAPI.Accounts.CA"
RUN dotnet build "BudgifyAPI.Accounts.CA.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BudgifyAPI.Accounts.CA.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BudgifyAPI.Accounts.CA.dll"]

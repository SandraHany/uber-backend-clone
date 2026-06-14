FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

COPY src/shared/Uber.Sharedkernel/Uber.Sharedkernel.csproj       src/shared/Uber.Sharedkernel/
COPY src/shared/Uber.Shared/Uber.Shared.csproj                   src/shared/Uber.Shared/
COPY src/shared/Uber.Contracts/Uber.Contracts.csproj             src/shared/Uber.Contracts/
COPY src/shared/Uber.ServiceDefaults/Uber.ServiceDefaults.csproj src/shared/Uber.ServiceDefaults/
COPY src/services/Voyage/Uber.Voyage.API/Uber.Voyage.API.csproj                         src/services/Voyage/Uber.Voyage.API/
COPY src/services/Voyage/Uber.Voyage.Application/Uber.Voyage.Application.csproj         src/services/Voyage/Uber.Voyage.Application/
COPY src/services/Voyage/Uber.Voyage.Domain/Uber.Voyage.Domain.csproj                   src/services/Voyage/Uber.Voyage.Domain/
COPY src/services/Voyage/Uber.Voyage.Infrastructure/Uber.Voyage.Infrastructure.csproj   src/services/Voyage/Uber.Voyage.Infrastructure/
COPY Directory.Packages.props ./
COPY Directory.Build.props ./

RUN dotnet restore src/services/Voyage/Uber.Voyage.API/Uber.Voyage.API.csproj

COPY . .

RUN dotnet publish src/services/Voyage/Uber.Voyage.API/Uber.Voyage.API.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

RUN groupadd --system appgroup && useradd --system --gid appgroup appuser
USER appuser

COPY --from=build /app/publish .

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "Uber.Voyage.API.dll"]
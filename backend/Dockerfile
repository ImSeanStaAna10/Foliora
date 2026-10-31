# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY backend/Foliora.slnx .
COPY backend/src/Foliora.Domain/Foliora.Domain.csproj src/Foliora.Domain/
COPY backend/src/Foliora.Application/Foliora.Application.csproj src/Foliora.Application/
COPY backend/src/Foliora.Infrastructure/Foliora.Infrastructure.csproj src/Foliora.Infrastructure/
COPY backend/src/Foliora.Api/Foliora.Api.csproj src/Foliora.Api/
COPY backend/src/Foliora.Worker/Foliora.Worker.csproj src/Foliora.Worker/
COPY backend/tests/Foliora.UnitTests/Foliora.UnitTests.csproj tests/Foliora.UnitTests/
COPY backend/tests/Foliora.IntegrationTests/Foliora.IntegrationTests.csproj tests/Foliora.IntegrationTests/

# Restore dependencies
RUN dotnet restore Foliora.slnx

# Copy all source code
COPY backend/ .

# Build and publish API
WORKDIR /src/src/Foliora.Api
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Foliora.Api.dll"]

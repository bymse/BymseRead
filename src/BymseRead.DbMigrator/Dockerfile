# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy the solution file and project files first to leverage Docker cache for dependencies
COPY BymseRead.sln ./
COPY src/BymseRead.Core/*.csproj ./src/BymseRead.Core/
COPY src/BymseRead.Infrastructure/*.csproj ./src/BymseRead.Infrastructure/
COPY src/BymseRead.DbMigrator/*.csproj ./src/BymseRead.DbMigrator/
COPY src/Directory.Build.props ./src/

# Restore dependencies
RUN dotnet restore src/BymseRead.DbMigrator/BymseRead.DbMigrator.csproj

COPY src/BymseRead.Core/. ./src/BymseRead.Core/
COPY src/BymseRead.Infrastructure/. ./src/BymseRead.Infrastructure/
COPY src/BymseRead.DbMigrator/. ./src/BymseRead.DbMigrator/

# Build and publish the application
RUN dotnet publish src/BymseRead.DbMigrator/BymseRead.DbMigrator.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime
WORKDIR /app

# Copy the published application
COPY --from=build /app/publish .

# Set environment variables for production mode
ENV DOTNET_ENVIRONMENT=Production
ENV DOTNET_EnableDiagnostics=0
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# The entry point will be the worker service that runs migrations
ENTRYPOINT ["dotnet", "BymseRead.DbMigrator.dll"]
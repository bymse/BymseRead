# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy the solution file and project files first to leverage Docker cache for dependencies
COPY BymseRead.sln ./
COPY BymseRead.Core/*.csproj ./BymseRead.Core/
COPY BymseRead.Infrastructure/*.csproj ./BymseRead.Infrastructure/
COPY BymseRead.DbMigrator/*.csproj ./BymseRead.DbMigrator/
COPY Directory.Build.props ./

# Restore dependencies
RUN dotnet restore BymseRead.DbMigrator/BymseRead.DbMigrator.csproj

COPY BymseRead.Core/. ./BymseRead.Core/
COPY BymseRead.Infrastructure/. ./BymseRead.Infrastructure/
COPY BymseRead.DbMigrator/. ./BymseRead.DbMigrator/

# Build and publish the application
RUN dotnet publish BymseRead.DbMigrator/BymseRead.DbMigrator.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app

# Copy the published application
COPY --from=build /app/publish .

# Set environment variables for production mode
ENV DOTNET_ENVIRONMENT=Production
ENV DOTNET_EnableDiagnostics=0
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# The entry point will be the worker service that runs migrations
ENTRYPOINT ["dotnet", "BymseRead.DbMigrator.dll"]
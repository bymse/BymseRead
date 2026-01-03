# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy the solution file and project files first to leverage Docker cache for dependencies
COPY BymseRead.sln ./
COPY src/BymseRead.Core/*.csproj ./src/BymseRead.Core/
COPY src/BymseRead.Infrastructure/*.csproj ./src/BymseRead.Infrastructure/
COPY src/BymseRead.Service/*.csproj ./src/BymseRead.Service/
COPY src/Directory.Build.props ./src/

# Restore dependencies
RUN dotnet restore src/BymseRead.Service/BymseRead.Service.csproj

# Copy the rest of the source code
COPY src/BymseRead.Core/. ./src/BymseRead.Core/
COPY src/BymseRead.Infrastructure/. ./src/BymseRead.Infrastructure/
COPY src/BymseRead.Service/. ./src/BymseRead.Service/

# Build and publish the application
RUN dotnet publish src/BymseRead.Service/BymseRead.Service.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Install Ghostscript
RUN apt-get update && apt-get install -y ghostscript && rm -rf /var/lib/apt/lists/*

# Copy the published application
COPY --from=build /app/publish .

# ASP.NET Core environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_EnableDiagnostics=0

# Configure web server
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Expose port 80
EXPOSE 80

ENTRYPOINT ["dotnet", "BymseRead.Service.dll"]
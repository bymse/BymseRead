# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution file and project files first to leverage Docker cache for dependencies
COPY BymseRead.sln ./
COPY BymseRead.Core/*.csproj ./BymseRead.Core/
COPY BymseRead.Infrastructure/*.csproj ./BymseRead.Infrastructure/
COPY BymseRead.Service/*.csproj ./BymseRead.Service/
COPY Directory.Build.props ./

# Restore dependencies
RUN dotnet restore BymseRead.Service/BymseRead.Service.csproj

# Copy the rest of the source code
COPY BymseRead.Core/. ./BymseRead.Core/
COPY BymseRead.Infrastructure/. ./BymseRead.Infrastructure/
COPY BymseRead.Service/. ./BymseRead.Service/

# Build and publish the application
RUN dotnet publish BymseRead.Service/BymseRead.Service.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
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
# Build stage
FROM mcr.microsoft.com/devcontainers/dotnet

# Install Ghostscript
RUN apt-get update && apt-get install -y ghostscript && rm -rf /var/lib/apt/lists/*

# Expose port 80
EXPOSE 80

ENTRYPOINT ["dotnet", "BymseRead.Service.dll"]
# Use the .NET 8 SDK as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy and restore dependencies
COPY ["BloodBankManagementSystem.API/BloodBankManagementSystem.API.csproj", "BloodBankManagementSystem.API/"]
COPY ["BloodBankManagementSystem.Infrastructure/BloodBankManagementSystem.Infrastructure.csproj", "BloodBankManagementSystem.Infrastructure/"]
COPY ["BloodBankManagementSystem.Core/BloodBankManagementSystem.Core.csproj", "BloodBankManagementSystem.Core/"]
RUN dotnet restore "BloodBankManagementSystem.API/BloodBankManagementSystem.API.csproj"

# Copy everything and build
COPY . .
WORKDIR "/src/BloodBankManagementSystem.API"
RUN dotnet build -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BloodBankManagementSystem.API.dll"]

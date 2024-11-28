# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["InventoryManagementAPI.csproj", "InventoryManagementAPI/"]
RUN dotnet restore "InventoryManagementAPI/InventoryManagementAPI.csproj"

# Copy the rest of the application source code
COPY . InventoryManagementAPI/

WORKDIR "/src/InventoryManagementAPI"

# Build the project
RUN dotnet build "InventoryManagementAPI.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "InventoryManagementAPI.csproj" -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InventoryManagementAPI.dll"]

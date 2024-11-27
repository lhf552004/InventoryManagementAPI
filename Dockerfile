FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["InventoryManagementAPI.csproj", "InventoryManagementAPI/"]
RUN dotnet restore "InventoryManagementAPI/InventoryManagementAPI.csproj"
COPY . .
WORKDIR "/src/InventoryManagementAPI"
RUN dotnet build "InventoryManagementAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InventoryManagementAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InventoryManagementAPI.dll"]

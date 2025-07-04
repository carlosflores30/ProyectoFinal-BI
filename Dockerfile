FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SmartStockAI.Api/SmartStockAI.Api.csproj", "SmartStockAI.Api/"]
COPY ["SmartStockAI.Domain/SmartStockAI.Domain.csproj", "SmartStockAI.Domain/"]
COPY ["SmartStockAI.Infrastructure/SmartStockAI.Infrastructure.csproj", "SmartStockAI.Infrastructure/"]
COPY ["SmartStockAI.Application/SmartStockAI.Application.csproj", "SmartStockAI.Application/"]
RUN dotnet restore "SmartStockAI.Api/SmartStockAI.Api.csproj"
COPY . .
WORKDIR "/src/SmartStockAI.Api"
RUN dotnet build "./SmartStockAI.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SmartStockAI.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartStockAI.Api.dll"]

#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["services/AccountProcessService/AccountProcessService.Api/AccountProcessService.Api.csproj", "services/AccountProcessService/AccountProcessService.Api/"]
COPY ["buildingBlocks/Common.Logging/Common.Logging.csproj", "buildingBlocks/Common.Logging/"]
COPY ["services/AccountProcessService/AccountProcessService.Domain/AccountProcessService.Domain.csproj", "services/AccountProcessService/AccountProcessService.Domain/"]
COPY ["buildingBlocks/BuildingBlocks.Core.Domain/BuildingBlocks.Core.Domain.csproj", "buildingBlocks/BuildingBlocks.Core.Domain/"]
COPY ["buildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "buildingBlocks/EventBus.Messages/"]
COPY ["services/AccountProcessService/AccountProcessService.Infrastructure/AccountProcessService.Infrastructure.csproj", "services/AccountProcessService/AccountProcessService.Infrastructure/"]
COPY ["buildingBlocks/BuildingBlocks.Application/BuildingBlocks.Application.csproj", "buildingBlocks/BuildingBlocks.Application/"]
COPY ["buildingBlocks/BuildingBlocks.Infrastructure/BuildingBlocks.Infrastructure.csproj", "buildingBlocks/BuildingBlocks.Infrastructure/"]
COPY ["buildingBlocks/BuildingBlocks.Dapper/BuildingBlocks.Dapper.csproj", "buildingBlocks/BuildingBlocks.Dapper/"]
COPY ["services/AccountProcessService/AccountProcessService.Application/AccountProcessService.Application.csproj", "services/AccountProcessService/AccountProcessService.Application/"]
RUN dotnet restore "services/AccountProcessService/AccountProcessService.Api/AccountProcessService.Api.csproj"
COPY . .
WORKDIR "/src/services/AccountProcessService/AccountProcessService.Api"
RUN dotnet build "AccountProcessService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AccountProcessService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccountProcessService.Api.dll"]
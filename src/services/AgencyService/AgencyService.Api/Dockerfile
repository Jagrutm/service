#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["services/agencyService/AgencyService.Api/AgencyService.Api.csproj", "services/agencyService/AgencyService.Api/"]
COPY ["buildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "buildingBlocks/EventBus.Messages/"]
COPY ["services/agencyService/AgencyService.Infrastructure/AgencyService.Infrastructure.csproj", "services/agencyService/AgencyService.Infrastructure/"]
COPY ["buildingBlocks/BuildingBlocks.Dapper/BuildingBlocks.Dapper.csproj", "buildingBlocks/BuildingBlocks.Dapper/"]
COPY ["buildingBlocks/BuildingBlocks.Core.Domain/BuildingBlocks.Core.Domain.csproj", "buildingBlocks/BuildingBlocks.Core.Domain/"]
COPY ["services/agencyService/AgencyService.Application/AgencyService.Application.csproj", "services/agencyService/AgencyService.Application/"]
COPY ["services/agencyService/AgencyService.Domain/AgencyService.Domain.csproj", "services/agencyService/AgencyService.Domain/"]
COPY ["buildingBlocks/BuildingBlocks.Application/BuildingBlocks.Application.csproj", "buildingBlocks/BuildingBlocks.Application/"]
COPY ["buildingBlocks/BuildingBlocks.Infrastructure/BuildingBlocks.Infrastructure.csproj", "buildingBlocks/BuildingBlocks.Infrastructure/"]
COPY ["buildingBlocks/Common.Logging/Common.Logging.csproj", "buildingBlocks/Common.Logging/"]
RUN dotnet restore "services/agencyService/AgencyService.Api/AgencyService.Api.csproj"
COPY . .
WORKDIR "/src/services/agencyService/AgencyService.Api"
RUN dotnet build "AgencyService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AgencyService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AgencyService.Api.dll"]
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["./Dislinkt.Connections/Dislinkt.Connections.WebApi/Dislinkt.Connections.WebApi.csproj", "Dislinkt.Connections.WebApi/"]
COPY ["./Dislinkt.Connections/Dislinkt.Connections.Application/Dislinkt.Connections.Application.csproj", "Dislinkt.Connections.Application/"]
COPY ["./Dislinkt.Connections/Dislinkt.Connections.Core/Dislinkt.Connections.Core.csproj", "Dislinkt.Connections.Core/"]
COPY ["./Dislinkt.Connections/Dislinkt.Connections.Domain/Dislinkt.Connections.Domain.csproj", "Dislinkt.Connections.Domain/"]
COPY ["./Dislinkt.Connections/Dislinkt.Connections/Dislinkt.Connections.csproj", "Dislinkt.Connections/"]
COPY ["./Dislinkt.Connections/Dislinkt.Connections.Persistence.MongoDB/Dislinkt.Connections.Persistence.MongoDB.csproj", "Dislinkt.Connections.Persistence.MongoDB/"]
COPY ["./Dislinkt.Connections/Dislinkt.Connections.Persistence.Neo4j/Dislinkt.Connections.Persistence.Neo4j.csproj", "Dislinkt.Connections.Persistence.Neo4j/"]

COPY . .
WORKDIR "./Dislinkt.Connections/Dislinkt.Connections.WebApi"
RUN dotnet build "Dislinkt.Connections.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dislinkt.Connections.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dislinkt.Connections.WebApi.dll"]
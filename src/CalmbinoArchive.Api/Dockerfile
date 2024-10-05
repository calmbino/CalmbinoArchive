FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CalmbinoArchive.Api/CalmbinoArchive.Api.csproj", "src/CalmbinoArchive.Api/"]
COPY ["src/CalmbinoArchive.Domain/CalmbinoArchive.Domain.csproj", "src/CalmbinoArchive.Domain/"]
COPY ["src/CalmbinoArchive.Application/CalmbinoArchive.Application.csproj", "src/CalmbinoArchive.Application/"]
COPY ["src/CalmbinoArchive.Infrastructure/CalmbinoArchive.Infrastructure.csproj", "src/CalmbinoArchive.Infrastructure/"]
COPY ["src/CalmbinoArchive.Persistence/CalmbinoArchive.Persistence.csproj", "src/CalmbinoArchive.Persistence/"]
RUN dotnet restore "src/CalmbinoArchive.Api/CalmbinoArchive.Api.csproj"
COPY . .
WORKDIR "/src/src/CalmbinoArchive.Api"
RUN dotnet build "CalmbinoArchive.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CalmbinoArchive.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CalmbinoArchive.Api.dll"]

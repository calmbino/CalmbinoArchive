FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CalmbinoArchive.Web/CalmbinoArchive.Web/CalmbinoArchive.Web.Client/CalmbinoArchive.Web.Client.csproj", "src/CalmbinoArchive.Web/CalmbinoArchive.Web/CalmbinoArchive.Web.Client/"]
RUN dotnet restore "src/CalmbinoArchive.Web/CalmbinoArchive.Web/CalmbinoArchive.Web.Client/CalmbinoArchive.Web.Client.csproj"
COPY . .
WORKDIR "/src/src/CalmbinoArchive.Web/CalmbinoArchive.Web/CalmbinoArchive.Web.Client"
RUN dotnet build "CalmbinoArchive.Web.Client.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CalmbinoArchive.Web.Client.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CalmbinoArchive.Web.Client.dll"]

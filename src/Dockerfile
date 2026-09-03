ARG EMAIL_TEMPLATES_VERSION=1.1.1

FROM ghcr.io/planara/planara-email-templates:${EMAIL_TEMPLATES_VERSION} AS templates

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Planara.Notifications.csproj", "./"]
RUN dotnet restore "Planara.Notifications.csproj"

COPY . .
RUN dotnet build "Planara.Notifications.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Planara.Notifications.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
COPY --from=templates /templates ./templates

ENTRYPOINT ["dotnet", "Planara.Notifications.dll"]
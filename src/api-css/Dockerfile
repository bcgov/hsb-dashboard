ARG BUILD_CONFIGURATION=Release
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

RUN apt-get update && apt-get -y upgrade
RUN apt-get clean

EXPOSE 443 8080

# Copy csproj and restore as distinct layers
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /src/api-css
COPY src/api-css/ .
COPY src/libs/ /src/libs/

RUN dotnet restore
RUN dotnet publish "HSB.CSS.API.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish

# Runtime image
FROM base AS final

WORKDIR /app
COPY --from=build /app/publish .

# USER appuser
ENTRYPOINT ["dotnet", "HSB.CSS.API.dll"]
# ENTRYPOINT ["tail", "-f", "/dev/null"]

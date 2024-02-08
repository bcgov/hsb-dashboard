ARG ASPNETCORE_ENVIRONMENT=Release
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

RUN apt-get update && apt-get -y upgrade
RUN apt-get -y install curl libc6-dev libgdiplus
RUN apt-get clean

EXPOSE 443 8080

# Copy csproj and restore as distinct layers
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

ENV PATH="$PATH:/root/.dotnet/tools"
# RUN dotnet tool install --global dotnet-ef --version 8.0.0

WORKDIR /src/api
COPY src/api/ .
COPY src/libs/ /src/libs/

RUN dotnet restore
RUN dotnet publish "HSB.API.csproj" -c "$ASPNETCORE_ENVIRONMENT" -o /app/publish

# Runtime image
FROM base AS final

WORKDIR /app
COPY --from=build /app/publish .
COPY src/api/entrypoint.sh .
RUN chmod +x /app/entrypoint.sh

# USER appuser
USER 1001
# ENTRYPOINT ["/app/entrypoint.sh"]
ENTRYPOINT ["dotnet", "HSB.API.dll"]

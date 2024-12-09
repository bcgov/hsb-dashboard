ARG ASPNETCORE_ENVIRONMENT=Release
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim-amd64 AS base

RUN apt-get update && apt-get -y upgrade
RUN apt -y install curl libc6-dev libgdiplus
RUN apt-get clean

EXPOSE 443 8080

# Copy csproj and restore as distinct layers
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /src/data-service
COPY src/data-service/ .
COPY src/libs/ /src/libs/

RUN dotnet restore "HSB.DataService.csproj"
RUN dotnet publish "HSB.DataService.csproj" -c "$ASPNETCORE_ENVIRONMENT" -o /app/publish

# Runtime image
FROM base AS final

WORKDIR /app
COPY --from=build /app/publish .
COPY --from=build src/data-service/entrypoint.sh .
RUN chmod +x /app/entrypoint.sh

USER 1001
# ENTRYPOINT ["/app/entrypoint.sh"]
# ENTRYPOINT ["tail", "-f", "/dev/null"]
ENTRYPOINT ["dotnet", "HSB.DataService.dll"]

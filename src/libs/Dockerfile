
FROM mcr.microsoft.com/dotnet/sdk:8.0

ENV DOTNET_CLI_HOME=/tmp
ENV PATH="$PATH:/tmp/.dotnet/tools"
ENV ASPNETCORE_ENVIRONMENT=Production
ENV MIGRATION=

# Switch to root for package installs
USER 0
RUN dotnet tool install --global dotnet-ef --version 8.0.0

WORKDIR /src
COPY . .

RUN cd /src/dal &&\
  dotnet restore &&\
  dotnet build

RUN fix_permissions() { while [ $# -gt 0 ]; do chgrp -R 0 "$1" && chmod -R g=u "$1"; shift; done } && \
    fix_permissions "/src" "/tmp"

RUN chmod -R 0777 /src
RUN mkdir /.local
RUN chmod -R 0777 /.local

# Run container by default as user with id 1001 (default)
USER 1001

ENTRYPOINT cd /src/dal && \
  dotnet ef migrations list && \
  dotnet ef database update $MIGRATION -v
# ENTRYPOINT ["tail", "-f", "/dev/null"]

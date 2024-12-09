#!/bin/bash

. ./scripts/os.sh

# On Mac, use localhost instead of host.docker.internal

if [ "$(uname)" == "Darwin" ]; then
    export dockerHost=localhost
else
    export dockerHost=host.docker.internal
fi



#######################################################
# Database Variables
#######################################################
export dbName=hsb
export dbUser=$("$GREP" -Po 'POSTGRES_USER=\K.*$' ./database/.env 2>/dev/null)

export dbPassword=$("$GREP" -Po 'POSTGRES_PASSWORD=\K.*$' ./database/.env 2>/dev/null)

export dbPort=$("$GREP" -Po 'DB_PORT=\K.*$' ./.env 2>/dev/null)
if [ -z "$dbPort" ]
then
    dbPort=30000
    export dbPort
fi

export keycloakUser=$("$GREP" -Po 'KEYCLOAK_USER=\K.*$' ./keycloak/.env 2>/dev/null)

export keycloakPassword=$("$GREP" -Po 'KEYCLOAK_PASSWORD=\K.*$' ./keycloak/.env 2>/dev/null)

export privateKey=$("$GREP" -Po 'NEXTAUTH_SECRET=\K.*$' ./src/dashboard/.env 2>/dev/null)
if [ -z "$privateKey" ]
then
    privateKey=$(date +%s | sha256sum | base64 | head -c 32)
    export privateKey
fi

#######################################################
# Docker Environment Variables
#######################################################

export portKeycloakHttp=$("$GREP" -Po 'KEYCLOAK_HTTP_PORT=\K.*$' ./.env 2>/dev/null)
if [ -z "$portKeycloakHttp" ]
then
    portKeycloakHttp=30001
    export portApiHttp
fi
export portKeycloakHttps=$("$GREP" -Po 'KEYCLOAK_HTTPS_PORT=\K.*$' ./.env 2>/dev/null)
if [ -z "$portKeycloakHttps" ]
then
    portKeycloakHttps=30002
    export portApiHttps
fi

export portCssApiHttp=$("$GREP" -Po 'CSS_HTTP_PORT=\K.*$' ./.env 2>/dev/null)
if [ -z "$portCssApiHttp" ]
then
    portCssApiHttp=30003
    export portCssApiHttp
fi
export portCssApiHttps=$("$GREP" -Po 'CSS_HTTPS_PORT=\K.*$' ./.env 2>/dev/null)
if [ -z "$portCssApiHttps" ]
then
    portCssApiHttps=30004
    export portCssApiHttps
fi

export portApiHttp=$("$GREP" -Po 'API_HTTP_PORT=\K.*$' ./.env 2>/dev/null)
if [ -z "$portApiHttp" ]
then
    portApiHttp=30005
    export portApiHttp
fi
export portApiHttps=$("$GREP" -Po 'API_HTTPS_PORT=\K.*$' ./.env 2>/dev/null)
if [ -z "$portApiHttps" ]
then
    portApiHttps=30006
    export portApiHttps
fi

export portAppHttp=$("$GREP" -Po 'APP_HTTP_PORT=\K.*$' ./.env 2>/dev/null)
if [ -z "$portAppHttp" ]
then
    portAppHttp=30080
    export portAppHttp
fi
export portAppHttps=$("$GREP" -Po 'APP_HTTPS_PORT=\K.*$' ./.env 2>/dev/null)
if [ -z "$portAppHttps" ]
then
    portAppHttps=30443
    export portAppHttps
fi

export portNginxHttp=$("$GREP" -Po 'NGINX_HTTP_PORT=\K.*$' ./.env 2>/dev/null)
if [ -z "$portNginxHttp" ]
then
    portNginxHttp=30007
    export portNginxHttp
fi
export portNginxHttps=$("$GREP" -Po 'NGINX_HTTPS_PORT=\K.*$' ./.env 2>/dev/null)
if [ -z "$portNginxHttps" ]
then
    portNginxHttps=30008
    export portNginxHttps
fi

#!/bin/bash

gen_env () {
  action=${1-}

  if [ "$action" = "refresh" ]; then
    echo "Deleting environment files"
    if [ -f ".env" ]; then mv .env .bak.env 2>/dev/null; true; fi
    if [ -f "database/.env" ]; then mv database/.env database/.bak.env 2>/dev/null; true; fi
    if [ -f "keycloak/.env" ]; then mv keycloak/.env keycloak/.bak.env 2>/dev/null; true; fi
    if [ -f "src/api-css/.env" ]; then mv src/api-css/.env src/api-css/.bak.env 2>/dev/null; true; fi
    if [ -f "src/api/.env" ]; then mv src/api/.env src/api/.bak.env 2>/dev/null; true; fi
    if [ -f "src/libs/dal/.env" ]; then mv src/libs/dal/.env src/libs/dal/.bak.env 2>/dev/null; true; fi
    if [ -f "src/dashboard/.env" ]; then mv src/dashboard/.env src/dashboard/.bak.env 2>/dev/null; true; fi
    if [ -f "nginx/.env" ]; then mv nginx/.env nginx/.bak.env 2>/dev/null; true; fi
  fi

  echo "Generating environment files"
  gen_root_env ${1-}
  gen_db_env ${1-}
  gen_keycloak_env ${1-}
  gen_css_api_env ${1-}
  gen_api_env ${1-}
  gen_dal_env ${1-}
  gen_app_env ${1-}
  gen_nginx_env ${1-}
}

gen_root_env () {
  if test -f "./.env"; then
    echo "./.env exists"
  else
    echo \
"DB_PORT=$dbPort

KEYCLOAK_HTTP_PORT=$portKeycloakHttp
KEYCLOAK_HTTPS_PORT=$portKeycloakHttps

CSS_HTTP_PORT=$portCssApiHttp
CSS_HTTPS_PORT=$portCssApiHttps

API_HTTP_PORT=$portApiHttp
API_HTTPS_PORT=$portApiHttps

APP_HTTP_PORT=$portAppHttp
APP_HTTPS_PORT=$portAppHttps

NGINX_HTTP_PORT=$portNginxHttp
NGINX_HTTPS_PORT=$portNginxHttps" >> ./.env
    echo "./.env created"
  fi
}

gen_db_env () {
  if test -f "./database/.env"; then
    echo "./database/.env exists"
  else
    echo \
"POSTGRES_DB=$dbName
POSTGRES_PASSWORD=$dbPassword
POSTGRES_USER=$dbUser
POSTGRES_ADMIN_PASSWORD=$dbPassword
KEYCLOAK_DB=keycloak" >> ./database/.env
    echo "./database/.env created"
  fi
}

gen_keycloak_env () {
  if test -f "./keycloak/.env"; then
    echo "./keycloak/.env exists"
  else
    echo \
"PROXY_ADDRESS_FORWARDING=true
KEYCLOAK_USER=$keycloakUser
KEYCLOAK_PASSWORD=$keycloakPassword
KEYCLOAK_IMPORT='/tmp/realm-export.json -Dkeycloak.profile.feature.scripts=enabled -Dkeycloak.profile.feature.upload_scripts=enabled'
KEYCLOAK_LOGLEVEL=WARN
ROOT_LOGLEVEL=WARN

DB_VENDOR=POSTGRES
DB_ADDR=database
DB_PORT=5432
DB_SCHEMA=public
DB_DATABASE=keycloak
DB_USER=$dbUser
DB_PASSWORD=$dbPassword" >> ./keycloak/.env
    echo "./keycloak/.env created"
  fi
}

gen_css_api_env () {
  if test -f "./src/api-css/.env"; then
    echo "./src/api-css/.env exists"
  else
    echo \
"# API
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://+:8080

# Keycloak API
Keycloak__Secret={GET FROM KEYCLOAK}" >> ./src/api-css/.env
    echo "./src/api-css/.env created"
  fi
}

gen_api_env () {
  if test -f "./src/api/.env"; then
    echo "./src/api/.env exists"
  else
    echo \
"# API
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://+:8080

# Database
ConnectionStrings__Default=Host=$dockerHost:$dbPort;Include Error Detail=true;Log Parameters=true;
DB_NAME=$dbName
DB_USER=$dbUser
DB_PASSWORD=$dbPassword

# Authentication
Keycloak__Authority=http://$dockerHost:$portKeycloakHttp/auth/realms/hsb
Keycloak__Audience=hsb-app,hsb-service-account
Keycloak__Issuer=hsb-app,hsb-service-account
CSS__ApiUrl=http://$dockerHost:$portCssApiHttp/api
CSS__Authority=http://$dockerHost:$portCssApiHttp
CSS__TokenPath=/api/v1/token
CSS__ClientId={GET FROM CSS}
CSS__Secret={GET FROM CSS}" >> ./src/api/.env
    echo "./src/api/.env created"
  fi
}

gen_dal_env () {
  if test -f "./src/libs/dal/.env"; then
    echo "./src/libs/dal/.env exists"
  else
    echo \
"ConnectionStrings__Default=Host=$dockerHost:$dbPort;Database=$dbName;Include Error Detail=true;Log Parameters=true;
POSTGRES_USER=$dbUser
POSTGRES_PASSWORD=$dbPassword" >> ./src/libs/dal/.env
    echo "./src/libs/dal/.env created"
  fi
}

gen_app_env () {
  if test -f "./src/dashboard/.env"; then
    echo "./src/dashboard/.env exists"
  else
    echo \
"KEYCLOAK_DEBUG=true
KEYCLOAK_CLIENT_ID=hsb-app
KEYCLOAK_SECRET={GET FROM KEYCLOAK}
KEYCLOAK_ISSUER=http://host.docker.internal:$portKeycloakHttp/auth/realms/hsb
KEYCLOAK_END_SESSION_PATH=/protocol/openid-connect/logout

NEXTAUTH_URL=http://localhost:$portAppHttp
NEXTAUTH_SECRET=$privateKey
" >> ./src/dashboard/.env
    echo "./src/dashboard/.env created"
  fi
}

gen_nginx_env () {
  if test -f "./nginx/.env"; then
    echo "./nginx/.env exists"
  else
    echo \
"" >> ./nginx/.env
    echo "./nginx/.env created"
  fi
}

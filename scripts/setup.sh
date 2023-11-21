#!/bin/bash

gen_env () {
  action=${1-}

  if [ "$action" = "refresh" ]; then
    echo "Deleting environment files"
    if [ -f ".env" ]; then rm .env; fi
    if [ -f "database/.env" ]; then rm database/.env; fi
    if [ -f "src/api-css/.env" ]; then rm src/api-css/.env; fi
    if [ -f "src/api/.env" ]; then rm src/api/.env; fi
    if [ -f "src/libs/dal/.env" ]; then rm src/libs/dal/.env; fi
    if [ -f "src/dashboard/.env" ]; then rm src/dashboard/.env; fi
    if [ -f "src/nginx/.env" ]; then rm src/nginx/.env; fi
    if [ -f "keycloak/.env" ]; then rm keycloak/.env; fi
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
"DB_PORT=$portDb

KEYCLOAK_HTTP_PORT=$portKeycloakHttp
KEYCLOAK_HTTPS_PORT=$portKeycloakHttps

CSS_API_HTTP_PORT=$portCssApiHttp
CSS_API_HTTPS_PORT=$portCssApiHttps

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
ConnectionStrings__Default=Host=$dockerHost:$portDb;Include Error Detail=true;Log Parameters=true;
DB_NAME=$dbName
DB_USER=$dbUser
DB_PASSWORD=$dbPassword

# Authentication
Authentication__Issuer=http://localhost:$portAppHttp/
Authentication__Audience=http://localhost:$portAppHttp/
Authentication__PrivateKey=$privateKey
Authentication__SaltLength=$saltLength
Authentication__AccessTokenExpiresIn=00:05:00
Authentication__RefreshTokenExpiresIn=01:00:00

# Mail
Mail__FromEmail=
Mail__Password=" >> ./src/api/.env
    echo "./src/api/.env created"
  fi
}

gen_dal_env () {
  if test -f "./src/libs/dal/.env"; then
    echo "./src/libs/dal/.env exists"
  else
    echo \
"ConnectionStrings__Default=Host=$dockerHost:$portDb;Database=$dbName;Include Error Detail=true;Log Parameters=true;
POSTGRES_USER=$dbUser
POSTGRES_PASSWORD=$dbPassword

DEFAULT_PASSWORD=$defaultPassword
SALT_LENGTH=$saltLength" >> ./src/libs/dal/.env
    echo "./src/libs/dal/.env created"
  fi
}

gen_app_env () {
  if test -f "./src/dashboard/.env"; then
    echo "./src/dashboard/.env exists"
  else
    echo \
"" >> ./src/dashboard/.env
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

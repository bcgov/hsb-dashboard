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

  dbUser=admin
  # if [[ -z "$dbUser" ]]
  # then
  #     echo 'Enter a username for the database.'
  #     read -p 'Username: ' dbUser
  #     export dbUser
  # fi

  if [[ -z "$dbPassword" ]]
  then
      # Generate a random password that satisfies password requirements.
      echo 'A random database password is being generated.'
      dbPassword=$(date +%s | sha256sum | base64 | head -c 29)A8!
      echo "Your generated password is: $dbPassword"
      export dbPassword
  fi

  if [[ -z "$keycloakUser" ]]
  then
      echo 'Enter an admin username for your local Keycloak installation:'
      read -p 'Username: ' keycloakUser
      export keycloakUser
  fi

  if [[ -z "$keycloakPassword" ]]
  then
      echo "Enter a password for the local Keycloak user \"$keycloakUser\":"
      read -p 'Password: ' keycloakPassword
      export keycloakPassword
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
  gen_data_service_env ${1-}
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
KEYCLOAK_LOGLEVEL=WARN
ROOT_LOGLEVEL=WARN

KC_DB=postgres
KC_DB_URL=jdbc:postgresql://database/keycloak
KC_DB_USERNAME=$dbUser
KC_DB_PASSWORD=$dbPassword
KC_HOSTNAME=localhost
KEYCLOAK_ADMIN=$keycloakUser
KEYCLOAK_ADMIN_PASSWORD=$keycloakPassword" >> ./keycloak/.env
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
Keycloak__ClientId={GET FROM Keycloak Client UID}
Keycloak__Secret={GET FROM Keycloak Client Credentials Secret}" >> ./src/api-css/.env
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
Keycloak__RequireHttpsMetadata=false
Keycloak__Authority=http://$dockerHost:$portKeycloakHttp/realms/hsb
Keycloak__Audience=hsb-app,hsb-service-account
Keycloak__Issuer=hsb-app,hsb-service-account
Keycloak__Secret={GET FROM KEYCLOAK}
CSS__ApiUrl=http://$dockerHost:$portCssApiHttp/api
CSS__Authority=http://$dockerHost:$portCssApiHttp
CSS__TokenPath=/api/v1/token
CSS__ClientId={GET FROM CSS}
CSS__Secret={GET FROM CSS}

###################################
# Common Single Sign-On
###################################
# Keycloak__Authority=https://dev.loginproxy.gov.bc.ca/realms/standard
# Keycloak__Audience={GET FROM CSS}
# Keycloak__Issuer={GET FROM CSS}
# Keycloak__Secret={GET FROM CSS}
# CSS__Environment=dev
# CSS__ApiUrl=https://api.loginproxy.gov.bc.ca
# CSS__Authority=https://loginproxy.gov.bc.ca
# CSS__ClientId={GET FROM CSS}
# CSS__Secret={GET FROM CSS}" >> ./src/api/.env
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
KEYCLOAK_ISSUER=http://host.docker.internal:$portKeycloakHttp/realms/hsb
KEYCLOAK_END_SESSION_PATH=/protocol/openid-connect/logout
KEYCLOAK_TOKEN_URL=/protocol/openid-connect/token

NEXTAUTH_URL=http://localhost:$portAppHttp
NEXTAUTH_SECRET=$privateKey
NEXT_WEBPACK_USEPOLLING=false

API_URL=http://host.docker.internal:$portApiHttp

# Use to fake authentication and authorization.
# NEXT_PUBLIC_AUTH_STATUS=authenticated
# NEXT_PUBLIC_AUTH_ROLES=hsb

###################################
# Common Single Sign-On
###################################
# KEYCLOAK_ISSUER=https://dev.loginproxy.gov.bc.ca/auth/realms/standard
# KEYCLOAK_CLIENT_ID={GET FROM CSS}
# KEYCLOAK_SECRET={GET FROM CSS}" >> ./src/dashboard/.env
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

gen_data_service_env () {
  if test -f "./src/data-service/.env"; then
    echo "./src/data-service/.env exists"
  else
    echo \
"# Service Now Configuration
ServiceNow__Instance={GET FROM SERVICE NOW}
ServiceNow__Username={GET FROM SERVICE NOW}
ServiceNow__Password={GET FROM SERVICE NOW}

# HSB API Configuration
Service__ApiUrl=http://host.docker.internal:$portApiHttp

Keycloak__RequireHttpsMetadata=false
Keycloak__Authority=http://host.docker.internal:$portKeycloakHttp/realms/hsb
Keycloak__Audience=hsb-app
Keycloak__Issuer=hsb-app
Keycloak__Secret={GET FROM KEYCLOAK}

CHES__AuthUrl=https://dev.loginproxy.gov.bc.ca/auth/realms/comsvcauth/protocol/openid-connect/token
CHES__HostUri=https://ches-dev.api.gov.bc.ca/api/v1
CHES__Username={GET FROM CHES}
CHES__Password={GET FROM CHES}" >> ./src/data-service/.env
    echo "./src/data-service/.env created"
  fi
}

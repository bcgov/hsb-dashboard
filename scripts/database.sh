#!/bin/bash

# Change directory to the DAL, or back to root
change-dir () {
  if [[ ! "$(pwd)" =~ .*"src/libs/dal" ]]; then
    cd src/libs/dal
  else
    cd ../../../
  fi
}

tool-update () {
  package=${1-"dotnet-ef"}
	dotnet tool update --global $package
}

db-migration () {
  cd src/libs
  if [[ "$(docker inspect hsb:db-migration > /dev/null 2>&1 && echo 'yes' || echo 'no')" == "yes" ]]; then
    docker image rm hsb:db-migration -f
  fi
  if [[ "$(docker inspect hsb-db-migration > /dev/null 2>&1 && echo 'yes' || echo 'no')" == "yes" ]]; then
    docker rm hsb-db-migration -f
  fi
  docker build -t hsb:db-migration . --no-cache --force-rm
  docker run -i --env-file=dal/.env --name hsb-db-migration hsb:db-migration
  docker rm hsb-db-migration -f
  docker image rm hsb:db-migration -f
  cd ../../
}

db-migrations () {
  change-dir
  dotnet ef migrations list
  change-dir
}

db-add () {
  if [ -z "${1-}" ]; then
    echo "$0: The migration name is required"
    exit 4
  fi
  echo "Create a new migration with the specified name '$1'"
  change-dir
  dotnet ef migrations add $1
  code -r ./Migrations/*_$1.cs
  bash ../../scripts/db-migration.sh $1
  change-dir
}

# Update the database with the latest migration (n=name of migration).
db-update () {
  echo "Update the database to the specified migration '${1-"latest"}'"
  change-dir
  dotnet ef database update ${1-} --verbose
  change-dir
}

db-rollback () {
  if [ -z "${1-}" ]; then
    echo "$0: The migration name is required"
    exit 4
  fi
  echo "Rollback to the specified migration '$1'"
  db-update $1
}

db-remove () {
  echo "Delete the last migration files"
  change-dir
	dotnet ef migrations remove --force;
  change-dir
}

db-drop () {
  echo "Drop the database"
  change-dir
  dotnet ef database drop --force;
  change-dir
}

db-refresh () {
  echo "Refresh the database, drop and recreate"
  db-drop
  db-update
}

db-redo () {
  if [ -z "${1-}" ]; then
    echo "$0: The migration name is required"
    exit 4
  fi
  db-rollback 0
  db-remove
  db-add $1
  db-update
}

#!/bin/bash

# Run the specified docker service, or the profile, or all of them.
docker_up () {
  profile="${p:-main}"
  echo "docker -p $profile up -d $s"
  docker-compose\
    -f docker-compose.yaml\
    --profile $profile\
    up -d $s
}

# Build the specified docker service, or all of them.
docker_build () {
  profile="${p:-all}"
  echo "docker -p $profile build $s"
  docker-compose\
    -f docker-compose.yaml\
    --profile $profile\
    build --force-rm $s
    # Original: build --no-cache --force-rm $s
}

# Stop the specified docker service, or all of them.
docker_stop () {
  profile="${p:-all}"
  echo "docker -p $profile stop $s"
  docker-compose\
    -f docker-compose.yaml\
    --profile $profile\
    stop $s
}

# Stop and remove the specified docker service, or all of them.
docker_down () {
  profile="${p:-all}"
  echo "docker -p $profile down $s"
  docker-compose\
    -f docker-compose.yaml\
    --profile $profile\
    down $s
}

# Remove the specified docker services and images.
docker_remove () {
  echo "Removing container hsb-$1"
  docker rm -f hsb-$1
  docker image rm -f hsb:$1
}

# Stop, and start the specified docker service.
docker_restart () {
  if [ -z "${1-}" ]; then
    echo "$0: The service name is required"
    exit 4
  fi
  docker_stop $1
  docker_up $1
}

# Stop, remove, build and run the specified docker service.
docker_refresh () {
  if [ -z "${1-}" ]; then
    echo "$0: The service name is required"
    exit 4
  fi
  docker_stop $1
  docker_remove $1
  docker_build $1
  docker_up $1
}

# Nuke the environment
docker_nuke () {
  profile="${p:-all}"
  docker-compose \
    -f docker-compose.yaml\
    --profile $profile\
    down -v
  docker-compose rm -f -v
  docker image prune -f
  docker rmi hsb:database hsb:keycloak hsb:api hsb:app
}

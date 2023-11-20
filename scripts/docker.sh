#!/bin/bash

# Run the specified docker service, or the profile, or all of them.
docker_up () {
  echo "docker up $s"
  if [ ! -z "$p" ] && [ ! -z "$s" ]; then
    docker-compose\
      -f docker-compose.yaml\
      --profile $p\
      up -d $s
  else
    docker-compose\
      -f docker-compose.yaml\
      --profile all\
      up -d $s
  fi
}

# Build the specified docker service, or all of them.
docker_build () {
  echo "docker build $s"
  docker-compose\
    -f docker-compose.yaml\
    --profile all\
    build --no-cache --force-rm $s
}

# Stop the specified docker service, or all of them.
docker_stop () {
  echo "docker stop $s"
  docker-compose\
    -f docker-compose.yaml\
    --profile all\
    stop $s
}

# Stop and remove the specified docker service, or all of them.
docker_down () {
  echo "docker down $s"
  docker-compose\
    -f docker-compose.yaml\
    --profile all\
    down $s
}

# Remove all the docker services and images.
docker_remove () {
  echo "Removing container hsb-$1"
  docker rm -f hsb-$1
  docker image rm -f hsb:$1
}

# Stop, remove, build and run the specified docker service.
docker_refresh () {
  if [ -z "${1-}" ]; then
    echo "$0: The service name is required"
    exit 4
  fi
  docker_stop
  docker_remove $1
  docker_build
  docker_up
}

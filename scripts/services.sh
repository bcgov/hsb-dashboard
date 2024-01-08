#!/bin/bash

data-service () {
  if [[ "$(docker inspect hsb:data-service > /dev/null 2>&1 && echo 'yes' || echo 'no')" == "yes" ]]; then
    docker image rm hsb:data-service -f
  fi
  if [[ "$(docker inspect hsb-data-service > /dev/null 2>&1 && echo 'yes' || echo 'no')" == "yes" ]]; then
    docker rm hsb-data-service -f
  fi
  docker build -t hsb:data-service -f src/data-service/Dockerfile . --no-cache --force-rm
  docker run -i --env-file=src/data-service/.env --name hsb-data-service hsb:data-service
  docker rm hsb-data-service -f
  docker image rm hsb:data-service -f
}

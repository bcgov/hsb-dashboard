#!/bin/bash

db-connect () {
  die () {
    echo >&2 "$@"
    exit 1
  }

  if [ ! $1 ]; then
    die "First argument require to specify the environment [dev,test,prod,tools]."
  fi

  POD_NAME=$(oc get pods -n e89443-$1 --selector name=database --no-headers -o custom-columns=POD:.metadata.name)

  if [ ! -z "$POD_NAME" ]; then
    oc port-forward $POD_NAME ${2:-22222}:${3:-1433} -n e89443-$1
  else
    echo "Pod not found"
  fi
}

deploy () {
  image=${1-}
  tag=${2-'latest'}
  env=${3-}

  # Extract the random characters of the project namespace.
  project=$(oc project --short); project=${project//-[a-z]*/}; echo $project
  # Change the current environment
  oc project $project-tools
  # login
  docker login -u $(oc whoami) -p $(oc whoami -t) image-registry.apps.emerald.devops.gov.bc.ca/$project-tools

  if [ "$image" = "api" ]; then
    # buid and deploy api
    docker build -f src/api/Dockerfile -t image-registry.apps.emerald.devops.gov.bc.ca/$project-tools/api:$tag .
    docker push image-registry.apps.emerald.devops.gov.bc.ca/$project-tools/api:$tag

    if [ ! -z "$env" ]; then
      oc tag api:$tag api:$env
    fi
  elif [ "$image" = "app" ]; then
    # buid and deploy app
    docker build -f src/dashboard/Dockerfile.prod -t image-registry.apps.emerald.devops.gov.bc.ca/$project-tools/dashboard:$tag ./src/dashboard
    docker push image-registry.apps.emerald.devops.gov.bc.ca/$project-tools/dashboard:$tag

    if [ ! -z "$env" ]; then
      oc tag dashboard:$tag dashboard:$env
    fi
  fi
}

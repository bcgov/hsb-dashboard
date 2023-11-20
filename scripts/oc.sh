#!/bin/bash

db-connect () {
  die () {
    echo >&2 "$@"
    exit 1
  }

  if [ ! $1 ]; then
    die "First argument require to specify the environment [dev,test,prod,tools]."
  fi

  POD_NAME=$(oc get pods -n 9b301c-$1 --selector name=database --no-headers -o custom-columns=POD:.metadata.name)

  if [ ! -z "$POD_NAME" ]; then
    oc port-forward $POD_NAME ${2:-22222}:${3:-1433} -n 9b301c-$1
  else
    echo "Pod not found"
  fi
}


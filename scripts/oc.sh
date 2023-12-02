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

  IMAGE_REGISTRY=${4-'image-registry.apps.emerald.devops.gov.bc.ca'}

  # Extract the random characters of the project namespace.
  project=$(oc project --short); project=${project//-[a-z]*/}; echo $project
  # Change the current environment
  oc project $project-tools
  # login
  docker login -u $(oc whoami) -p $(oc whoami -t) $IMAGE_REGISTRY/$project-tools

  if [ "$image" = "api" ]; then
    # buid and deploy api
    docker build -f src/api/Dockerfile -t $IMAGE_REGISTRY/$project-tools/$image:$tag .
    docker push $IMAGE_REGISTRY/$project-tools/$image:$tag

    if [ ! -z "$env" ]; then
      oc tag $image:$tag $image:$env
    fi
  elif [ "$image" = "app" ]; then
    # buid and deploy app
    image=dashboard
    docker build -f src/dashboard/Dockerfile.prod -t $IMAGE_REGISTRY/$project-tools/$image:$tag ./src/dashboard
    docker push $IMAGE_REGISTRY/$project-tools/$image:$tag

    if [ ! -z "$env" ]; then
      oc tag $image:$tag $image:$env
    fi
  elif [ "$image" = "db" ]; then
    # buid and deploy app
    image=db-migration
    docker build -f src/libs/Dockerfile -t $IMAGE_REGISTRY/$project-tools/$image:$tag ./src/libs
    docker push $IMAGE_REGISTRY/$project-tools/$image:$tag
    migration=${4-}

        # --rm \
    if [ ! -z "$env" ]; then
      oc tag $image:$tag $image:$env
      echo "Running database migration in $project-$env"
      overrides="{
          \"apiVersion\":\"v1\",
          \"spec\":{
            \"containers\":[
              {
                \"name\":\"$image\",
                \"image\":\"$IMAGE_REGISTRY/$project-tools/$image:$env\",
                \"env\":[
                  {
                    \"name\":\"MIGRATION\",
                    \"value\":\"$migration\"
                  },
                  {
                    \"name\":\"ConnectionStrings__Default\",
                    \"valueFrom\":{
                      \"secretKeyRef\":{
                        \"name\":\"database\",
                        \"key\":\"CONNECTION_STRING\"
                      }
                    }
                  },
                  {
                    \"name\":\"POSTGRES_USER\",
                    \"valueFrom\":{
                      \"secretKeyRef\":{
                        \"name\":\"database\",
                        \"key\":\"DB_USER\"
                      }
                    }
                  },
                  {
                    \"name\":\"POSTGRES_PASSWORD\",
                    \"valueFrom\":{
                      \"secretKeyRef\":{
                        \"name\":\"database\",
                        \"key\":\"DB_PASSWORD\"
                      }
                    }
                  }
                ],
                \"labels\":{
                  \"name\":\"db-migration\",\"part-of\":\"hsb\",\"component\":\"database\",\"DataClass\":\"Low\"
                  },
                \"resources\":{
                  \"requests\":{
                    \"memory\":\"250Mi\",
                    \"cpu\":\"50m\"
                  },
                  \"limits\":{
                    \"memory\":\"500Mi\",
                    \"cpu\":\"250m\"
                  }
                },
                \"imagePullPolicy\": \"Always\"
              }
            ]
          }
        }"
      oc run $image \
        -n $project-$env \
        --image=$image \
        --image-pull-policy=Always \
        --attach \
        --rm \
        --labels='name=db-migration,part-of=hsb,component=database,DataClass=Low' \
        --restart=Never \
        --timeout=10m \
        --override-type='merge' \
        --overrides="$overrides"
    fi
  fi
}


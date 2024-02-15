#!/bin/bash

IMAGE_REGISTRY='image-registry.apps.emerald.devops.gov.bc.ca'

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

oc-build () {
  image=${1-}
  tag=${2-'latest'}

  # Extract the random characters of the project namespace.
  project=$(oc project --short); project=${project//-[a-z]*/}; echo $project

  if [ "$image" = "api" ]; then
    # buid api
    docker build -f src/api/Dockerfile -t $IMAGE_REGISTRY/$project-tools/$image:$tag .
  elif [ "$image" = "app" ]; then
    # buid app
    image=dashboard
    docker build -f src/dashboard/Dockerfile.prod -t $IMAGE_REGISTRY/$project-tools/$image:$tag ./src/dashboard
  elif [ "$image" = "db" ]; then
    # buid app
    image=db-migration
    docker build -f src/libs/Dockerfile -t $IMAGE_REGISTRY/$project-tools/$image:$tag ./src/libs
  elif [ "$image" = "data-service" ]; then
    # buid data-service
    image=data-service
    docker build -f src/data-service/Dockerfile -t $IMAGE_REGISTRY/$project-tools/$image:$tag .
  fi
}

oc-push () {
  image=${1-}
  tag=${2-'latest'}

  # Extract the random characters of the project namespace.
  project=$(oc project --short); project=${project//-[a-z]*/}; echo $project
  # Change the current environment
  oc project $project-tools
  # login
  docker login -u $(oc whoami) -p $(oc whoami -t) $IMAGE_REGISTRY/$project-tools

  if [ "$image" = "api" ]; then
    # buid and deploy api
    docker push $IMAGE_REGISTRY/$project-tools/$image:$tag
  elif [ "$image" = "app" ]; then
    # buid and deploy app
    image=dashboard
    docker push $IMAGE_REGISTRY/$project-tools/$image:$tag
  elif [ "$image" = "db" ]; then
    # buid and deploy db-migration
    image=db-migration
    docker push $IMAGE_REGISTRY/$project-tools/$image:$tag
  elif [ "$image" = "data-service" ]; then
    # buid and deploy data-service
    docker push $IMAGE_REGISTRY/$project-tools/$image:$tag
  fi
}

oc-deploy () {
  image=${1-}
  tag=${2-'latest'}
  env=${3-'dev'}

  # Extract the random characters of the project namespace.
  project=$(oc project --short); project=${project//-[a-z]*/}; echo $project
  # Change the current environment
  oc project $project-tools
  # login
  docker login -u $(oc whoami) -p $(oc whoami -t) $IMAGE_REGISTRY/$project-tools

  if [ "$image" = "api" ]; then
    oc tag $image:$tag $image:$env
  elif [ "$image" = "app" ]; then
    # buid and deploy app
    image=dashboard
    oc tag $image:$tag $image:$env
  elif [ "$image" = "data-service" ]; then
    # buid and deploy data-service
    oc tag $image:$tag $image:$env
  elif [ "$image" = "db" ]; then
    # buid and deploy app
    image=db-migration
    oc tag $image:$tag $image:$env
  fi
}

oc-run () {
  image=${1-}
  env=${2-'dev'}

  # Extract the random characters of the project namespace.
  project=$(oc project --short); project=${project//-[a-z]*/}; echo $project
  # Change the current environment
  oc project $project-tools
  # login
  docker login -u $(oc whoami) -p $(oc whoami -t) $IMAGE_REGISTRY/$project-tools

  if [ "$image" = "data-service" ]; then
    echo "Running data-service in $project-$env"
    overrides="{
        \"apiVersion\":\"v1\",
        \"spec\":{
          \"containers\":[
            {
              \"name\":\"$image\",
              \"image\":\"$IMAGE_REGISTRY/$project-tools/$image:$env\",
              \"env\":[
                {
                  \"name\":\"Logging__LogLevel__HSB\",
                  \"value\":\"Debug\"
                },
                {
                  \"name\":\"HTTP_PROXY\",
                  \"value\":\"http://swpxkam.gov.bc.ca:8080\"
                },
                {
                  \"name\":\"HTTPS_PROXY\",
                  \"value\":\"http://swpxkam.gov.bc.ca:8080\"
                },
                {
                  \"name\":\"NO_PROXY\",
                  \"value\":\".cluster.local,.svc,10.91.0.0/16,172.30.0.0/16,127.0.0.1,localhost,.gov.bc.ca,api\"
                },
                {
                  \"name\":\"Keycloak__Authority\",
                  \"valueFrom\":{
                    \"configMapKeyRef\":{
                      \"name\":\"keycloak\",
                      \"key\":\"KEYCLOAK_AUTHORITY\"
                    }
                  }
                },
                {
                  \"name\":\"Keycloak__Audience\",
                  \"valueFrom\":{
                    \"configMapKeyRef\":{
                      \"name\":\"keycloak\",
                      \"key\":\"KEYCLOAK_AUDIENCE\"
                    }
                  }
                },
                {
                  \"name\":\"Keycloak__Issuer\",
                  \"valueFrom\":{
                    \"configMapKeyRef\":{
                      \"name\":\"keycloak\",
                      \"key\":\"KEYCLOAK_ISSUER\"
                    }
                  }
                },
                {
                  \"name\":\"Keycloak__Secret\",
                  \"valueFrom\":{
                    \"secretKeyRef\":{
                      \"name\":\"keycloak\",
                      \"key\":\"KEYCLOAK_CLIENT_SECRET\"
                    }
                  }
                },
                {
                  \"name\":\"Service__ApiUrl\",
                  \"valueFrom\":{
                    \"configMapKeyRef\":{
                      \"name\":\"data-service\",
                      \"key\":\"API_URL\"
                    }
                  }
                },
                {
                  \"name\":\"ServiceNow__ApiUrl\",
                  \"valueFrom\":{
                    \"secretKeyRef\":{
                      \"name\":\"service-now\",
                      \"key\":\"URL\"
                    }
                  }
                },
                {
                  \"name\":\"ServiceNow__Instance\",
                  \"valueFrom\":{
                    \"secretKeyRef\":{
                      \"name\":\"service-now\",
                      \"key\":\"INSTANCE\"
                    }
                  }
                },
                {
                  \"name\":\"ServiceNow__Username\",
                  \"valueFrom\":{
                    \"secretKeyRef\":{
                      \"name\":\"service-now\",
                      \"key\":\"USERNAME\"
                    }
                  }
                },
                {
                  \"name\":\"ServiceNow__Password\",
                  \"valueFrom\":{
                    \"secretKeyRef\":{
                      \"name\":\"service-now\",
                      \"key\":\"PASSWORD\"
                    }
                  }
                }
              ],
              \"labels\":{
                \"name\":\"data-service\",\"part-of\":\"hsb\",\"component\":\"service\",\"DataClass\":\"Low\"
                },
              \"resources\":{
                \"requests\":{
                  \"memory\":\"250Mi\",
                  \"cpu\":\"50m\"
                },
                \"limits\":{
                  \"memory\":\"1Gi\",
                  \"cpu\":\"500m\"
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
      --labels='name=data-service,part-of=hsb,component=service,DataClass=Low' \
      --restart=Never \
      --timeout=10m \
      --override-type='merge' \
      --overrides="$overrides"
  elif [ "$image" = "db" ]; then
    # buid and deploy app
    image=db-migration
    migration=${3-}

    echo "Running database migration '$migration' in $project-$env"
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
}

IMAGE_REGISTRY='image-registry.apps.emerald.devops.gov.bc.ca'

# Extract the random characters of the project namespace.
project=$(oc project --short); project=${project//-[a-z]*/}; echo $project
image=db-migration
tag=latest

# docker pull $IMAGE_REGISTRY/$project-tools/$image:$tag

docker run $IMAGE_REGISTRY/$project-tools/$image:$tag

# docker rmi --force $IMAGE_REGISTRY/$project-tools/$image:$tag
# docker build -f src/libs/Dockerfile -t $IMAGE_REGISTRY/$project-tools/$image:$tag ./src/libs --no-cache

# oc project $project-tools
# docker login -u $(oc whoami) -p $(oc whoami -t) $IMAGE_REGISTRY/$project-tools

# docker push $IMAGE_REGISTRY/$project-tools/$image:$tag


# docker tag $IMAGE_REGISTRY/$project-tools/$image:$tag $IMAGE_REGISTRY/$project-tools/$image-2:$tag

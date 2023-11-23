# Openshift Helpful Commands

## Build Image

Build an image and give it a specific name and tag.

```bash
docker build -f src/dashboard/Dockerfile.prod -t dashboard:latest ./src/dashboard
```

## Find images

If you need to find images hosted in a image registry within Openshift.

```bash
# List all images in bcgov namespace
oc -n bcgov get is
# Search images
oc -n bcgov get imagestreamtag | grep $imageName
```

## Import an Image from Redhat

Some Redhat images are only accessible if they are first imported through Openshift.

```bash
oc import-image postgresql-13 --from=registry.redhat.io/rhel8/postgresql-13 --confirm -n e89443-tools
```

## Push/Pull Images with Docker

Login first.

```bash
# Login with Docker (insecure)
docker login -u $(oc whoami) -p $(oc whoami -t) image-registry.apps.emerald.devops.gov.bc.ca/e89443-tools
# Or login securely
oc whoami -t | docker login -u $(oc whoami) --password-stdin image-registry.apps.emerald.devops.gov.bc.ca/e89443-tools
```

## Push Image

If you have a local image you can push up to Openshift.

```bash
# List images
docker images
# Tag the image you want to push
docker tag $imageName:$tag image-registry.apps.emerald.devops.gov.bc.ca/e89443-tools/$imageName:$tag
# Push to image registry in Openshift
docker push image-registry.apps.emerald.devops.gov.bc.ca/e89443-tools/$imageName:$tag
```

## Pull Image

If you want to pull down an image from Openshift.

```bash
# List image
oc get is -n e89443-tools
# Pull image from Openshift
docker pull image-registry.apps.silver.devops.gov.bc.ca/e89443-tools/$imageName:$tag
```

## Test Network in Container

Sometimes you may need to confirm you container can communicate with the internet.

```bash
timeout 5 bash -c "</dev/tcp/google.com/443"; echo $?
```

## Change Resource Requirements

If you need to update the resource requirements of pods you can do this without editing their templates.

```bash
oc set resources dc/${DeployConfig.name} --requests=cpu=50m,memory=50Mi --limits=cpu=500m,memory=500Mi
```

## Extract the current project name without the environment

Sometimes it's a pain to remember the random names of the project, it's nice to use a command that extracts it.

```bash
# Extract the random characters of the project namespace.
project=$(oc project --short); project=${project//-[a-z]*/}; echo $project

# Change the current environment
oc project $project-tools
```

## Copy Files to or from a Container

You can use the CLI to copy local files to or from a remote directory in a container.
More information [here](https://docs.openshift.com/container-platform/3.11/dev_guide/copy_files_to_container.html)

```bash
oc rsync <source> <destination> [-c <container>]

# Copy to pod
oc rsync /home/user/source devpod1234:/src

# Copy from pod
oc rsync devpod1234:/src /home/user/source
```

If you need to create a pod that mounts a PVC first.

```bash
oc run some-pod --overrides='{"spec": {"containers": [{"command": ["/bin/bash", "-c", "trap : TERM INT; sleep infinity & wait"], "image": "registry.access.redhat.com/rhel7/rhel:latest", "name": "some-pod", "volumeMounts": [{"mountPath": "/data", "name": "some-data"}]}], "volumes": [{"name": "some-data", "persistentVolumeClaim": {"claimName": "test-file"}}]}}' --image=dummy --restart=Never
```

## Helpful Information on Docker Permissions

(Documentation)[https://developers.redhat.com/blog/2020/10/26/adapting-docker-and-kubernetes-containers-to-run-on-red-hat-openshift-container-platform#executable_permissions]

## Open a remote shell to containers

(Documentat)[https://docs.openshift.com/container-platform/3.11/dev_guide/ssh_environment.html]

```bash
oc rsh <pod>
```

## Delete all Pods in Error State

```bash
for pod in $(oc get pods | grep Error | awk '{print $1}'); do oc delete pod --grace-period=1 ${pod}; done
```

## Get Image Hash for Pod

```bash
oc get pods api-0 -o jsonpath="{..imageID}"
```

# Kustomize Setup Infrastructure

## Emerald Information

The Emerald Openshift realm is locked down and does not allow any network communication by default - [information](https://digital.gov.bc.ca/cloud/services/private/internal-resources/emerald/).

## Network Policies

Give permission to each namespace to pull images from the TOOLS namespace.

```bash
oc policy add-role-to-user system:image-puller system:serviceaccount:e89443-dev:default -n e89443-tools
```

## Setup Environment

Run the following command to create the objects in DEV.

```bash
# Create new objects
oc kustomize ./devops/kustomize/overlays/dev | oc create -f - --save-config=true

# Update existing objects
oc kustomize ./devops/kustomize/overlays/dev | oc apply -f -

# Delete all objects
oc kustomize ./devops/kustomize/overlays/dev | oc delete -f -
```

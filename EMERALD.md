# Emerald Realm

The Emerald Openshift Realm is designed to be secure (Zero Trust). As such, there are limitations and effort required to enable many of the common features that are automatically available in the Silver/Gold Realms.

More information can be found on the [Official Guide for Emerald Teams](https://digital.gov.bc.ca/cloud/services/private/internal-resources/emerald/).

## Important Differences

1. All network traffic must be explicitly enabled through Network Policies. Allow egress and ingress for every component appropriately.
2. External network traffic must have appropriate firewall rules, which may require a request in the Rocket Chat Channel. These firewall rules require your pods to use the appropriate proxy configuration rules. You may need your own custom `NO_PROXY` rule to enable your component the ability to communicate.
3. All network traffic must include security classification labels. This can be challenging to ensure the correct objects contain the labels.
4. DevOps CI/CD is limited. Webhooks are disabled which makes automation challenging. There are ways to make webhooks work through a public domain name. Additionally, building images that are not within the approved image registries is impossible. You will need to push all required base images to Artifactory, or your local Openshift image registry.

## Infrastructure as Code

All components following the Infrastructure as Code principles and can be found in the `./devops` folder.

Kustomize is a cli tool that helps provide a simple way to override configuration for specific environments. It has similarities to Helm.

Before running these commands you will need to create and populate configuration files for each environment. These contains secretes and are not included in source control.

`./devops/kustomize/overlays/secrets`

There is a folder for each environment, and within each folder it should contain the following files.

- css.env
- dashboard.env
- database.env
- database-local.env
- keycloak.env
- service-now.env

more information can be found at this [README](./devops/kustomize/overlays/secrets/README.md).

To deploy components to an environment you can use teh following command. This will generate the yaml required to create objects and then deploy them to the environment.

```bash
# Create all objects and deploy to DEV
oc kustomize ./devops/kustomize/overlays/dev | oc create -f -
```

To update with latest changes use the following command.

```bash
# Apply any changes to objects in DEV
oc kustomize ./devops/kustomize/overlays/dev | oc apply -f -
```

If you would like to only add a specific component, they are separated in their own folders.

```bash
# Create only objects associated with the API and deploy to DEV
oc kustomize ./devops/kustomize/overlays/dev/api | oc create -f -
```

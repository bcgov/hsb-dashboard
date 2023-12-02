# TNO Monitoring Guide

## Access - Sysdig

To setup team access to Sysdig follow [BCGov Developer Hub Sysdig Onboarding](https://docs.developer.gov.bc.ca/sysdig-monitor-setup-team/)

We created the Kubernetes CRD template with required information and applied the template TNO `*-tools` namespace.

For furture team onboarding or any update will be added to `./tno-sysdig-access.yaml` file. Anyone with contributor access to namespace can publish the changes to `*-tools` namespace and it will reflected in sysdig.

```yaml
apiVersion: ops.gov.bc.ca/v1alpha1
kind: SysdigTeam
metadata:
  name: e89443-sysdigteam
  namespace: e89443-tools
spec:
  team:
    description: The Sysdig Team for the OpenShift Project Set e89443 - TNO
    users:
      - name: Jeremy.1.Foster@gov.bc.ca
        role: ROLE_TEAM_EDIT
      - name: Jeremy.1.Foster@gov.bc.ca
        role: ROLE_TEAM_STANDARD
      - name: Jeremy.1.Foster@gov.bc.ca
        role: ROLE_TEAM_READ
```

## Team Access

To access them:

- Log in to Sysdig like how you did just now.

- Navigate to the bottom left hand of the page to switch your team, which should be named as [PROJECT_SET_LICENSE_PLATE]-team.

![Select Teams Image](./images/my_teams.png 'Select Teams!')

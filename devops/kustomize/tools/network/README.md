# Network Information

[Emerald Information](https://digital.gov.bc.ca/cloud/services/private/internal-resources/emerald/)

To get the internal IP address of the namespace.

```bash
oc get namespace e89443-tools -o jsonpath='{.metadata.annotations.ncp\/subnet-0}{"\n"}'
```

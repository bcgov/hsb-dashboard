# Setup Solution Secrets

Create a `database.env` file with the following records.

```env
CONNECTION_STRING=Host=postgres-0:5432;Database=hsb;Include Error Detail=true;Log Parameters=true;
DB_NAME=hsb
DB_USER=admin
DB_PASSWORD={YOUR PASSWORD}
```

Create a `keycloak.env` file with the following records.

```env
KEYCLOAK_CLIENT_SECRET={KEYCLOAK SECRET HERE}
```

Create a `css.env` file with the following records.

```env
CSS_INTEGRATION_ID={CSS INTEGRATION ID HERE}
CSS_CLIENT_ID={CSS CLIENT ID HERE}
CSS_SECRET={CSS SECRET HERE}
```

# Setup Database

Create a `database.env` file with the following records.

```env
CONNECTION_STRING=Host={REMOTE HOST}:{REMOTE PORT};Database={REMOTE DB NAME};Include Error Detail=true;Log Parameters=true;
DB_NAME={REMOTE DB NAME}
DB_USER={REMOTE USERNAME}
DB_PASSWORD={REMOTE PASSWORD}
```

Create a `database-local.env` file with the following records.

```env
CONNECTION_STRING=Host=postgres-0:5432;Database={LOCAL DB NAME};Include Error Detail=true;Log Parameters=true;
DB_NAME={LOCAL DB NAME}
DB_USER={LOCAL USERNAME}
DB_PASSWORD={LOCAL PASSWORD}
```

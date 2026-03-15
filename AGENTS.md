## Starting the Backend

1. Once per session: generate dev config: `dotnet run --project src/BymseRead.Tools -- SetupDevConfig && echo "ok" || echo "error"`
2. Start the service: `dotnet run --project src/BymseRead.Service --launch-profile http`
3. Backend runs at `http://localhost:5299`
4. Swagger UI: `http://localhost:5299/swagger`

## Starting the Frontend

1. `cd src/WebClientApp && npm install && npm run dev`
2. Frontend runs at `http://read.bymse.local:5173`
3. For keycloak login use email `default@example.com` and password `default`

## Reaching Services

| Service    | Hostname         | Port | Tool |
|------------|------------------|------|------|
| PostgreSQL | postgres         | 5432 | psql |
| Keycloak   | keycloak         | 8080 | curl |
| MinIO API  | minio            | 9000 | curl |
| Backend    | localhost        | 5299 | curl |
| Frontend   | read.bymse.local | 5173 | curl |

Credentials can be found in @src/BymseRead.Service/appsettings.Development.json

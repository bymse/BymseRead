## Backend

Use one terminal for the backend.

1. From the repo root, run once per session:
   `dotnet run --project src/BymseRead.Tools -- SetupDevConfig`
2. Start the backend:
   `dotnet run --project src/BymseRead.Service --launch-profile http`
3. Keep that terminal open.
4. The backend is running when the logs show:
   `Now listening on: http://localhost:5299`
5. Exact backend check command:
   `curl -sS -o /dev/null -w '%{http_code}\n' http://localhost:5299/swagger/index.html`
6. Expected result:
   `200`
7. Do not check `http://localhost:5299/`.
   That URL returns `404 Not Found` even when the backend is running.

## Frontend

Use a second terminal for the frontend.

1. Go to the frontend:
   `cd src/WebClientApp`
2. Install packages:
   `npm install`
3. Start the frontend:
   `npm run dev`
4. Keep that terminal open.
5. The frontend is running when Vite shows:
   `http://read.bymse.local:5173/`
6. You can also check it with:
   `curl -I http://read.bymse.local:5173`

## Login

1. Open:
   `http://read.bymse.local:5173`
2. Sign in with:
   Email: `default@example.com`
   Password: `default`
3. After login you should land on:
   `http://read.bymse.local:5173/books`

## Reaching Services

| Service    | Hostname         | Port | Tool |
|------------|------------------|------|------|
| PostgreSQL | postgres         | 5432 | psql |
| Keycloak   | keycloak         | 8080 | curl |
| MinIO API  | minio            | 9000 | curl |
| Backend    | localhost        | 5299 | curl |
| Frontend   | read.bymse.local | 5173 | curl |

## Notes

1. Run backend and frontend in separate terminals.
2. Keep both processes running while using the app.
3. Local config is in:
   `src/BymseRead.Service/appsettings.Development.json`

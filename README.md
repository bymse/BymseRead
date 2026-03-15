# BymseRead

Online reader for PDF books

# Local dev setup

1. Install https://ghostscript.com/releases/gsdnld.html
2. Add the following to `/etc/hosts`:
   ```
   127.0.0.1 read.bymse.local postgres keycloak minio
   ```
3. Add http://read.bymse.local:5173/ to `chrome://flags/#unsafely-treat-insecure-origin-as-secure`
4. Frontend address: http://read.bymse.local:5173/
5. Default keycloak user: default:default


# Todo

1. Verify sessions refresh

2. Check if session validation is required for every api call

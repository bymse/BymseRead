#!/bin/bash

set -e

SCRIPT_DIR="$(dirname "$0")"
COMPOSE_FILE="$SCRIPT_DIR/.devcontainer/docker-compose.yml"
COMPOSE_CMD="docker compose -f $COMPOSE_FILE --profile local-run"

case "${1:-}" in
  --stop)
    echo "Stopping BymseRead services..."
    $COMPOSE_CMD down --remove-orphans
    echo "Services stopped and containers removed."
    ;;

  --clear)
    echo "Stopping BymseRead services and removing volumes..."
    $COMPOSE_CMD down --volumes --remove-orphans
    echo "Services stopped, containers and volumes removed."
    ;;

  *)
    echo "Starting BymseRead with local-run profile..."
    $COMPOSE_CMD up -d --force-recreate --renew-anon-volumes

    echo ""
    echo "Services started successfully!"
    echo ""
    echo "Available services:"
    echo "  - PostgreSQL: localhost:15432"
    echo "  - Keycloak: http://localhost:8080"
    echo "  - MinIO API: http://localhost:19000"
    echo "  - MinIO Console: http://localhost:19001"
    echo ""
    echo "Usage:"
    echo "  ./local-run.sh          - Start services"
    echo "  ./local-run.sh --stop   - Stop services and remove containers"
    echo "  ./local-run.sh --clear  - Stop services and remove containers and volumes"
    echo ""
    echo "To view logs, run: docker compose -f $COMPOSE_FILE --profile local-run logs -f"
    echo ""
    echo "Running dev config setup..."
    dotnet restore "$SCRIPT_DIR"
    dotnet run --project $SCRIPT_DIR/src/BymseRead.Tools -- SetupDevConfig
    echo "dotnet restore completed."
    ;;
esac

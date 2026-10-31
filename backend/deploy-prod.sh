#!/usr/bin/env bash
set -e

echo "=== Starting Foliora PROD Deployment ==="

# Image tag for PROD
IMAGE_TAG=${IMAGE_TAG:-main}
GHCR_IMAGE="ghcr.io/imseanstaana10/foliora-api:$IMAGE_TAG"

echo "Pulling latest PROD image: $GHCR_IMAGE"
docker pull "$GHCR_IMAGE"

echo "Updating and restarting PROD services with Supabase PROD configuration..."
cd "$(dirname "$0")"

export FOLRIOA_API_IMAGE="$GHCR_IMAGE"
docker compose -f docker-compose.prod.yml up -d --no-deps api

echo "Verifying PROD deployment health..."
docker ps --filter name=foliora-api-prod --format "{{.Status}}"

echo "=== Foliora PROD Deployment Completed Successfully ==="

#!/usr/bin/env bash
set -e

echo "=== Starting Foliora DEV Deployment ==="

# Image tag for DEV
IMAGE_TAG=${IMAGE_TAG:-develop}
GHCR_IMAGE="ghcr.io/imseanstaana10/foliora-api:$IMAGE_TAG"

echo "Pulling latest DEV image: $GHCR_IMAGE"
docker pull "$GHCR_IMAGE"

echo "Updating and restarting DEV services..."
cd "$(dirname "$0")"

# Ensure docker-compose uses the pulled image
export FOLRIOA_API_IMAGE="$GHCR_IMAGE"
docker compose up -d --no-deps api

echo "Verifying DEV deployment health..."
docker ps --filter name=foliora-api-dev --format "{{.Status}}"

echo "=== Foliora DEV Deployment Completed Successfully ==="

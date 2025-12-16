#!/bin/bash

set -e

source "$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/vars.sdk.sh" 
source "$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/vars.project.sh" "$1"
shift || true

# Copy the engine to the publish directory
mkdir -p "$PUBLISH_DIR"
cp -r "$ENGINE_DIR/"**.* "$PUBLISH_DIR"

# Publish the game to a subdirectory so the engine can find it at runtime
dotnet publish "$PROJECT_FILE" "$@" \
  /p:OutputPath="$MSBUILD_OUTPUT_PATH" \
  /p:PublishDir="$MSBUILD_PUBLISH_DIR" \
  /p:AppendTargetFrameworkToOutputPath=false \
  /p:AppendRuntimeIdentifierToOutputPath=false

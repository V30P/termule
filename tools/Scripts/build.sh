#!/bin/bash

set -e

source "$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/vars.project.sh" "$1"
shift || true

dotnet build "$PROJECT_FILE" "$@" \
  /p:OutputPath="$MSBUILD_OUTPUT_PATH" \
  /p:AppendTargetFrameworkToOutputPath=false \
  /p:AppendRuntimeIdentifierToOutputPath=false
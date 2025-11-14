#!/bin/bash

set -e

source "$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/constants.sh"

dotnet build "$PROJECT_ROOT" "$@" \
  /p:OutputPath="$BUILD_DIR" \
  /p:IntermediateOutputPath="$INTERMEDIATE_OUTPUT_DIR" \
  /p:AppendTargetFrameworkToOutputPath=false \
  /p:AppendRuntimeIdentifierToOutputPath=false
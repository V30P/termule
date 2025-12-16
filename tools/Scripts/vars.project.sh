#!/bin/bash
# shellcheck disable=SC2034

set -e

# These are sometimes needed outside of a project
MSBUILD_OUTPUT_PATH="bin/Termule/Debug"
MSBUILD_PUBLISH_DIR="bin/Termule/Release/Game"

# Look for a project file and fail if there isn't one
PROJECT_DIR="${1:-.}"      
PROJECT_FILE=$(find "$PROJECT_DIR" -maxdepth 1 -name "*.csproj" 2>/dev/null  | head -n 1)
[[ -z "$PROJECT_FILE" ]] && return 1

PROJECT_NAME=$(basename "$PROJECT_FILE" .csproj)

# Remember to use these, NOT the MSBuild ones
BUILD_DIR="$PROJECT_DIR/$MSBUILD_OUTPUT_PATH"
PUBLISH_DIR="$PROJECT_DIR/bin/Termule/Release"  
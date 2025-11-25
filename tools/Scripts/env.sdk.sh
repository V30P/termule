#!/bin/bash
# shellcheck disable=SC2034

set -e

SCRIPTS_DIR="$(dirname "${BASH_SOURCE[0]}")" 
TOOLS_DIR="$(dirname "$SCRIPTS_DIR")"
TEMPLATE_DIR="$TOOLS_DIR/Template" 
SDK_DIR="$(dirname "$TOOLS_DIR")"
ENGINE_DIR="$SDK_DIR/bin"

# Executable extension will differ by platform
LIBRARY="$ENGINE_DIR/Termule.dll";
EXECUTABLE=$(find "$ENGINE_DIR" -maxdepth 1 \
  \( -name "Termule" -o -name "Termule.exe" \) 2>/dev/null | head -n 1)
#!/bin/bash

SADDLEBAG_DIR="$(dirname "${BASH_SOURCE[0]}")"
SDK_DIR="$(dirname "$SADDLEBAG_DIR")"

LIBRARY="$SDK_DIR/Library/Termule.dll";
TEMPLATE_DIR="$SADDLEBAG_DIR/Template/";

# Bootstrappers extensions may differ by platform
BOOTSTRAPPER_LIBRARY="$SDK_DIR/Bootstrapper/build/TermuleBootstrapper.dll"
BUILD_BOOTSTRAPPER=$(find "$SDK_DIR/Bootstrapper/build/" -maxdepth 1 \
  \( -name "TermuleBootstrapper" -o -name "TermuleBootstrapper.exe" \) 2>/dev/null | head -n 1) # Symbols
PUBLISH_BOOTSTRAPPER=$(find "$SDK_DIR/Bootstrapper/publish/" -maxdepth 1 \
  \( -name "TermuleBootstrapper" -o -name "TermuleBootstrapper.exe" \) 2>/dev/null | head -n 1) # No symbols

PROJECT_FILE=$(find . -maxdepth 1 -name "*.csproj" | head -n 1)
PROJECT_DIR=$(dirname "$PROJECT_FILE")
PROJECT_NAME=$(basename "$PROJECT_FILE" .csproj)
BUILD_DIR="$PROJECT_DIR/bin/sb/build/"
PUBLISH_DIR="$PROJECT_DIR/bin/sb/publish/" 
INTERMEDIATE_OUTPUT_DIR="$PROJECT_DIR/obj/"

#!/bin/bash

set -e

source "$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/build.sh" "$@" 

# Create a copy of the bootstrapper for the game, maintaining its extension
ext="${PUBLISH_BOOTSTRAPPER##*.}"
if [[ "$PUBLISH_BOOTSTRAPPER" == *.* ]]; then
    ext=".$ext"
else
    ext=""
fi

bootstrapper="$PUBLISH_DIR/$PROJECT_NAME$ext"
mkdir -p "$PUBLISH_DIR"
cp "$PUBLISH_BOOTSTRAPPER" "$bootstrapper"

# Append the game dll and its length to the new bootstrapper
project_dll=$BUILD_DIR/$PROJECT_NAME.dll
cat "$project_dll" >> "$bootstrapper" 
size=$(stat -c%s "$project_dll")
perl -e 'print pack("V", shift)' "$size" >> "$bootstrapper"
#!/bin/bash

set -e

# shellcheck source=env.sdk.sh
source "$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/env.sdk.sh"
source "$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/env.project.sh" "$1"

# Make a build and pass it to the engine executable
source "$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/build.sh" "$@"
"$EXECUTABLE" "$BUILD_DIR"
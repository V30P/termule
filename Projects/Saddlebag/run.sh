#!/bin/bash

set -e

source "$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/build.sh"
"$BUILD_BOOTSTRAPPER" "$BUILD_DIR/$PROJECT_NAME.dll"
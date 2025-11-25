#!/bin/bash

set -e

source "$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/env.project.sh" "$1"

rm -rf "$BUILD_DIR"*
rm -rf "$PUBLISH_DIR"*

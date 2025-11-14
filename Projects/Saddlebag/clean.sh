#!/bin/bash

set -e

source "$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/constants.sh"

rm -rf "$BUILD_DIR"*
rm -rf "$PUBLISH_DIR"*
rm -rf "$INTERMEDIATE_OUTPUT_DIR"*
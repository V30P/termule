#!/bin/bash

set -e

# Parse args by separator 
run_args=()
build_args=()
game_args=()

mode="run" 
for arg in "$@"; do
    case "$arg" in
        --build)
            mode="build"
            ;;
        --game)
            mode="game"
            ;;
        *)
            case "$mode" in
                run)   run_args+=("$arg") ;;
                build) build_args+=("$arg") ;;
                game)  game_args+=("$arg") ;;
            esac
            ;;
    esac
done

# Get environmental variables
source "$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/vars.sdk.sh"
source "$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/vars.project.sh" "${run_args[0]:-}"

# Make a build and pass it to the engine executable
"$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/build.sh" "${run_args[0]:-}" "${build_args[@]}"
"$EXECUTABLE" "$BUILD_DIR" "${game_args[@]}"

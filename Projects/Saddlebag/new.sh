#!/bin/bash

set -e

source "$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/constants.sh"

project="$(pwd)/$1"
cp -r "$TEMPLATE_DIR" "$project"

subs=(
"__CSPROJ__:csproj"
"__PROJECT_NAME__:$1"
"__LIBRARY_PATH__:$LIBRARY"
"__BOOTSTRAPPER_LIBRARY_PATH__:$BOOTSTRAPPER_LIBRARY"
"__BUILD_BOOTSTRAPPER_PATH__:$BUILD_BOOTSTRAPPER"
"__PUBLISH_BOOTSTRAPPER_PATH__:$PUBLISH_BOOTSTRAPPER"
"__BUILD_SCRIPT_PATH__:$SADDLEBAG_DIR/build.sh"
)

for sub in "${subs[@]}"; do
    IFS=":" read -r old new <<< "$sub"

    grep -Rl --null "$old" "$project" | while IFS= read -r -d '' file; do
        sed -i "s|${old}|${new}|g" "$file"
    done

    find "$project" -depth -type f -name "*$old*" -print0 |
    while IFS= read -r -d '' file; do
        mv "$file" "${file//$old/$new}"
    done

    find "$project" -depth -type d -name "*$old*" -print0 |
    while IFS= read -r -d '' dir; do
        mv "$dir" "${dir//$old/$new}"
    done
done 


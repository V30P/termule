#!/bin/bash

get_relative_path() {
    realpath -m --relative-to="$project" "$1"
}

set -e

source "$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/tm-vars.sdk.sh"
source "$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/tm-vars.project.sh" || true # We need some MSBuild variables

# Duplicate and rename the template 
project="$(pwd)/$1"
cp -r "$TEMPLATE_DIR" "$project"

# Apply substitutions in the copied template
subs=(
"__PROJECT_NAME__:$1"
"__CSPROJ__:csproj"
"__LIBRARY__:$(get_relative_path "$LIBRARY")"
"__EXECUTABLE__:$(get_relative_path "$EXECUTABLE")"
"__BUILD_SCRIPT__:$(get_relative_path "$SCRIPTS_DIR/tm-build.sh")"
"__MSBUILD_OUTPUT_PATH__:$MSBUILD_OUTPUT_PATH"
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


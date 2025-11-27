#!/usr/bin/env bash
set -euo pipefail

# Get script directory
script_dir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
props_file="$script_dir/../IaphubSdkVersion.props"

# Parse version from IaphubSdkVersion.props
if [[ ! -f "$props_file" ]]; then
  echo "Error: IaphubSdkVersion.props not found at $props_file" >&2
  exit 1
fi

version=$(grep -o '<IaphubIosSdkVersion>[^<]*</IaphubIosSdkVersion>' "$props_file" | sed 's/<IaphubIosSdkVersion>\(.*\)<\/IaphubIosSdkVersion>/\1/')

if [[ -z "$version" ]]; then
  echo "Error: Could not extract IaphubIosSdkVersion from $props_file" >&2
  exit 1
fi

url="https://github.com/iaphub/iaphub-ios-sdk/releases/download/${version}/Iaphub.xcframework.zip"
dest="$script_dir/../NativeReferences/Iaphub.xcframework"
stamp="${dest}.version"

ensure_command() {
  if ! command -v "$1" >/dev/null 2>&1; then
    echo "Missing required tool: $1" >&2
    exit 1
  fi
}

ensure_command curl
ensure_command unzip

# If we already have the requested version, exit quickly
if [[ -f "$stamp" ]]; then
  current_version="$(cat "$stamp")"
  if [[ "$current_version" == "$version" && -d "$dest" ]]; then
    echo "Iaphub SDK $version already present at $dest"
    exit 0
  fi
fi

temp_dir="$(mktemp -d)"
archive="$temp_dir/Iaphub.xcframework.zip"

echo "Downloading Iaphub iOS SDK $version from $url"
curl -L --fail --silent --show-error "$url" -o "$archive"

echo "Unpacking xcframework to $dest"
rm -rf "$dest"
unzip -q "$archive" -d "$temp_dir"

# The zip contains Iaphub.xcframework at root
if [[ ! -d "$temp_dir/Iaphub.xcframework" ]]; then
  echo "Downloaded archive does not contain Iaphub.xcframework" >&2
  exit 1
fi

mkdir -p "$(dirname "$dest")"
mv "$temp_dir/Iaphub.xcframework" "$dest"
echo "$version" > "$stamp"

echo "Iaphub SDK $version installed at $dest"

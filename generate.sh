#!/bin/bash
# Generates C# wrapper code from GodotApplePlugins XML documentation

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
DOC_CLASSES="$SCRIPT_DIR/doc_classes"
OUTPUT="$SCRIPT_DIR/src/GodotApplePlugins.NET/Generated"

echo "=== GodotApplePlugins.NET Code Generator ==="
echo ""

# Build the generator
echo "Building generator..."
dotnet build "$SCRIPT_DIR/src/GodotApplePlugins.Generator" -c Release -v q

# Download latest XML docs from upstream
echo ""
echo "Downloading latest docs from upstream..."
dotnet run --project "$SCRIPT_DIR/src/GodotApplePlugins.Generator" -c Release --no-build -- update-docs "$DOC_CLASSES"

# Generate C# wrappers
echo ""
echo "Running generator..."
dotnet run --project "$SCRIPT_DIR/src/GodotApplePlugins.Generator" -c Release --no-build -- "$DOC_CLASSES" "$OUTPUT"

echo ""
echo "=== Generation complete ==="
